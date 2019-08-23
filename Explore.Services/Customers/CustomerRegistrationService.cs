using Explore.Core;
using Explore.Core.Domain.Customers;
using Explore.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explore.Services.Customers
{
    /// <summary>
    /// 用户注册服务
    /// </summary>
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        /// <param name="encryptionService">Encryption service</param>
        /// <param name="workContext">Work context</param>
        public CustomerRegistrationService(ICustomerService customerService,
            IEncryptionService encryptionService,
            IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
            this._workContext = workContext;
            this._customerSettings = customerSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 检查输入的密码是否与保存的密码匹配
        /// </summary>
        /// <param name="customerPassword">用户密码</param>
        /// <param name="enteredPassword">输入的密码</param>
        /// <returns>如果密码匹配，则为true；否则为false</returns>
        protected bool PasswordsMatch(CustomerPassword customerPassword, string enteredPassword)
        {
            if (customerPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (customerPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, "SHA1");
                    break;
            }

            return customerPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="usernameOrEmail">用户名或邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public virtual CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password)
        {
            var customer = _customerService.GetCustomerByUsername(usernameOrEmail);
                

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;
            //只有注册的才能登录
            if (!customer.IsAdmin())
                return CustomerLoginResults.NotRegistered;
            //检查用户是否被锁定
            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return CustomerLoginResults.LockedOut;

            if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), password))
            {
                //密码错误
                customer.FailedLoginAttempts++;
                if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                    customer.FailedLoginAttempts >= _customerSettings.FailedPasswordAllowedAttempts)
                {
                    //锁定
                    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_customerSettings.FailedPasswordLockoutMinutes);
                    //重置计数器
                    customer.FailedLoginAttempts = 0;
                }
                _customerService.UpdateCustomer(customer);

                return CustomerLoginResults.WrongPassword;
            }

            //更新登录详细信息
            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.RequireReLogin = false;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            _customerService.UpdateCustomer(customer);

            return CustomerLoginResults.Successful;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>结果</returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Customer == null)
                throw new ArgumentException("无法加载当前客户");

            var result = new CustomerRegistrationResult();
            if (request.Customer.IsSearchEngineAccount())
            {
                result.AddError("无法注册搜索引擎");
                return result;
            }
            if (request.Customer.IsBackgroundTaskAccount())
            {
                result.AddError("无法注册后台任务帐户");
                return result;
            }
            if (request.Customer.IsAdmin())
            {
                result.AddError("当前用户已注册");
                return result;
            }

            if (String.IsNullOrEmpty(request.Username))
            {
                result.AddError("Account.Register.Errors.UsernameIsNotProvided");
                return result;
            }

            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Password is not provided");
                return result;
            }

            //验证用户名唯一性
            if (_customerService.GetCustomerByUsername(request.Username) != null)
            {
                result.AddError("The specified username already exists");
                return result;
            }

            //此时请求有效
            request.Customer.Username = request.Username;

            var customerPassword = new CustomerPassword
            {
                Customer = request.Customer,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    customerPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(5);
                        customerPassword.PasswordSalt = saltKey;
                        customerPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, "SHA1");
                    }
                    break;
            }
            _customerService.InsertCustomerPassword(customerPassword);

            request.Customer.Active = request.IsApproved;

            //添加'Registered'权限
            var registeredRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Administrators);
            if (registeredRole == null)
                throw new ExploreException("'Registered' role could not be loaded");
            request.Customer.CustomerRoles.Add(registeredRole);
            //移除'Guests'权限
            var guestRole = request.Customer.CustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests);
            if (guestRole != null)
                request.Customer.CustomerRoles.Remove(guestRole);

            _customerService.UpdateCustomer(request.Customer);

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        //public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException("request");

        //    var result = new ChangePasswordResult();
        //    if (String.IsNullOrWhiteSpace(request.Email))
        //    {
        //        result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailIsNotProvided"));
        //        return result;
        //    }
        //    if (String.IsNullOrWhiteSpace(request.NewPassword))
        //    {
        //        result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordIsNotProvided"));
        //        return result;
        //    }

        //    var customer = _customerService.GetCustomerByEmail(request.Email);
        //    if (customer == null)
        //    {
        //        result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailNotFound"));
        //        return result;
        //    }

        //    if (request.ValidateRequest)
        //    {
        //        //request isn't valid
        //        if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), request.OldPassword))
        //        {
        //            result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
        //            return result;
        //        }
        //    }

        //    //check for duplicates
        //    if (_customerSettings.UnduplicatedPasswordsNumber > 0)
        //    {
        //        //get some of previous passwords
        //        var previousPasswords = _customerService.GetCustomerPasswords(customer.Id, passwordsToReturn: _customerSettings.UnduplicatedPasswordsNumber);

        //        var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
        //        if (newPasswordMatchesWithPrevious)
        //        {
        //            result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
        //            return result;
        //        }
        //    }

        //    //at this point request is valid
        //    var customerPassword = new CustomerPassword
        //    {
        //        Customer = customer,
        //        PasswordFormat = request.NewPasswordFormat,
        //        CreatedOnUtc = DateTime.UtcNow
        //    };
        //    switch (request.NewPasswordFormat)
        //    {
        //        case PasswordFormat.Clear:
        //            customerPassword.Password = request.NewPassword;
        //            break;
        //        case PasswordFormat.Encrypted:
        //            customerPassword.Password = _encryptionService.EncryptText(request.NewPassword);
        //            break;
        //        case PasswordFormat.Hashed:
        //            {
        //                var saltKey = _encryptionService.CreateSaltKey(5);
        //                customerPassword.PasswordSalt = saltKey;
        //                customerPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _customerSettings.HashedPasswordFormat);
        //            }
        //            break;
        //    }
        //    _customerService.InsertCustomerPassword(customerPassword);

        //    //publish event
        //    _eventPublisher.Publish(new CustomerPasswordChangedEvent(customerPassword));

        //    return result;
        //}

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        //public virtual void SetEmail(Customer customer, string newEmail, bool requireValidation)
        //{
        //    if (customer == null)
        //        throw new ArgumentNullException("customer");

        //    if (newEmail == null)
        //        throw new NopException("Email cannot be null");

        //    newEmail = newEmail.Trim();
        //    string oldEmail = customer.Email;

        //    if (!CommonHelper.IsValidEmail(newEmail))
        //        throw new NopException(_localizationService.GetResource("Account.EmailUsernameErrors.NewEmailIsNotValid"));

        //    if (newEmail.Length > 100)
        //        throw new NopException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailTooLong"));

        //    var customer2 = _customerService.GetCustomerByEmail(newEmail);
        //    if (customer2 != null && customer.Id != customer2.Id)
        //        throw new NopException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailAlreadyExists"));

        //    if (requireValidation)
        //    {
        //        //re-validate email
        //        customer.EmailToRevalidate = newEmail;
        //        _customerService.UpdateCustomer(customer);

        //        //email re-validation message
        //        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.EmailRevalidationToken, Guid.NewGuid().ToString());
        //        _workflowMessageService.SendCustomerEmailRevalidationMessage(customer, _workContext.WorkingLanguage.Id);
        //    }
        //    else
        //    {
        //        customer.Email = newEmail;
        //        _customerService.UpdateCustomer(customer);

        //        //update newsletter subscription (if required)
        //        if (!String.IsNullOrEmpty(oldEmail) && !oldEmail.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            foreach (var store in _storeService.GetAllStores())
        //            {
        //                var subscriptionOld = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(oldEmail, store.Id);
        //                if (subscriptionOld != null)
        //                {
        //                    subscriptionOld.Email = newEmail;
        //                    _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscriptionOld);
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Sets a customer username
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newUsername">New Username</param>
        //public virtual void SetUsername(Customer customer, string newUsername)
        //{
        //    if (customer == null)
        //        throw new ArgumentNullException("customer");

        //    if (!_customerSettings.UsernamesEnabled)
        //        throw new ExploreException("Usernames are disabled");

        //    newUsername = newUsername.Trim();

        //    if (newUsername.Length > 100)
        //        throw new ExploreException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameTooLong"));

        //    var user2 = _customerService.GetCustomerByUsername(newUsername);
        //    if (user2 != null && customer.Id != user2.Id)
        //        throw new ExploreException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameAlreadyExists"));

        //    customer.Username = newUsername;
        //    _customerService.UpdateCustomer(customer);
        //}

        #endregion
    }
}
