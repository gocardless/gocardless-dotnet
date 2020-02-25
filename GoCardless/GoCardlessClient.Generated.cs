using System;
using System.Collections.Generic;
using System.Text;
using GoCardless.Services;

namespace GoCardless
{
    public partial class GoCardlessClient
    {

        /// <summary>
        ///A service for working with bank details lookup resources.
        /// </summary>
        public BankDetailsLookupService BankDetailsLookups => new BankDetailsLookupService(this);

        /// <summary>
        ///A service for working with creditor resources.
        /// </summary>
        public CreditorService Creditors => new CreditorService(this);

        /// <summary>
        ///A service for working with creditor bank account resources.
        /// </summary>
        public CreditorBankAccountService CreditorBankAccounts => new CreditorBankAccountService(this);

        /// <summary>
        ///A service for working with currency exchange rate resources.
        /// </summary>
        public CurrencyExchangeRateService CurrencyExchangeRates => new CurrencyExchangeRateService(this);

        /// <summary>
        ///A service for working with customer resources.
        /// </summary>
        public CustomerService Customers => new CustomerService(this);

        /// <summary>
        ///A service for working with customer bank account resources.
        /// </summary>
        public CustomerBankAccountService CustomerBankAccounts => new CustomerBankAccountService(this);

        /// <summary>
        ///A service for working with customer notification resources.
        /// </summary>
        public CustomerNotificationService CustomerNotifications => new CustomerNotificationService(this);

        /// <summary>
        ///A service for working with event resources.
        /// </summary>
        public EventService Events => new EventService(this);

        /// <summary>
        ///A service for working with instalment schedule resources.
        /// </summary>
        public InstalmentScheduleService InstalmentSchedules => new InstalmentScheduleService(this);

        /// <summary>
        ///A service for working with mandate resources.
        /// </summary>
        public MandateService Mandates => new MandateService(this);

        /// <summary>
        ///A service for working with mandate import resources.
        /// </summary>
        public MandateImportService MandateImports => new MandateImportService(this);

        /// <summary>
        ///A service for working with mandate import entry resources.
        /// </summary>
        public MandateImportEntryService MandateImportEntries => new MandateImportEntryService(this);

        /// <summary>
        ///A service for working with mandate pdf resources.
        /// </summary>
        public MandatePdfService MandatePdfs => new MandatePdfService(this);

        /// <summary>
        ///A service for working with payment resources.
        /// </summary>
        public PaymentService Payments => new PaymentService(this);

        /// <summary>
        ///A service for working with payout resources.
        /// </summary>
        public PayoutService Payouts => new PayoutService(this);

        /// <summary>
        ///A service for working with payout item resources.
        /// </summary>
        public PayoutItemService PayoutItems => new PayoutItemService(this);

        /// <summary>
        ///A service for working with redirect flow resources.
        /// </summary>
        public RedirectFlowService RedirectFlows => new RedirectFlowService(this);

        /// <summary>
        ///A service for working with refund resources.
        /// </summary>
        public RefundService Refunds => new RefundService(this);

        /// <summary>
        ///A service for working with subscription resources.
        /// </summary>
        public SubscriptionService Subscriptions => new SubscriptionService(this);

    }
}
