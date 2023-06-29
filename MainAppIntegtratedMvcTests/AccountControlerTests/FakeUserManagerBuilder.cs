using Moq;
using static MainAppIntegtratedMvcTests.AccountControlerTests.AccountControllerTests;

namespace MainAppIntegtratedMvcTests.AccountControlerTests
{
    public class FakeUserManagerBuilder
    {
        private Mock<FakeUserManager> _mock = new Mock<FakeUserManager>();

        public FakeUserManagerBuilder With(Action<Mock<FakeUserManager>> mock)
        {
            mock(_mock);
            return this;
        }

        public Mock<FakeUserManager> Build()
        {
            return _mock;
        }
    }
}
