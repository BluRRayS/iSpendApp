using System;
using iSpendDAL;
using iSpendLogic;
using iSpendUnitTests.MockContexts;
using Xunit;

namespace iSpendUnitTests
{
    public class AccountTests
    {
        private AccountLogic _accountLogic;

        public AccountTests()
        {
            _accountLogic = new AccountLogic(new AccountContextMock());
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
