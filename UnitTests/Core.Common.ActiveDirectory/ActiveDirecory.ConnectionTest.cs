using System;
using Sisyphus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sisyphus.ActiveDirectory;
using Sisyphus.ActiveDirectory.Exceptions;

namespace Core.Common.ActiveDirectoryUnitTest
{
    [TestClass]
    public class ActiveDirectoryUnitTest
    {

        class ActiveDirecotoryHelperMock : Sisyphus.ActiveDirectory.ActiveDirecotoryHelper
        {
            public static string CorrectDomainName => "correct.domain";
            public static string CorrectUserName => "user";
            public static string CorrectPassword => "usercorrectpassword";
        }

        [TestMethod]
        public void TestConnection()
        {
            var activeDirecotoryMock = new ActiveDirecotoryHelperMock();
            activeDirecotoryMock.Connect(ActiveDirecotoryHelperMock.CorrectDomainName, ActiveDirecotoryHelperMock.CorrectUserName, ActiveDirecotoryHelperMock.CorrectPassword);
            Assert.AreEqual(true, ((IActiveDirectory)activeDirecotoryMock).Connected);
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectionErrorException))]
        public void TestConnectionException()
        {
            var activeDirecotoryMock = new ActiveDirecotoryHelperMock();
            activeDirecotoryMock.Connect("incorrectdomainname", "incorrectdomainuser", "incorrectpassword");
            Assert.AreEqual(true, activeDirecotoryMock.Connected);
        }
    }
}
