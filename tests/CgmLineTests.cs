using codessentials.CGM.Classes;
using FluentAssertions;
using NUnit.Framework;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    public class CgmLineTests
    {
        [Test]
        public void Sets_Line_When_A_Is_Before_B()
        {
            var a = new CgmPoint(0, 0);
            var b = new CgmPoint(10, 10);

            var line = new CgmLine(a, b);

            line.A.X.Should().Be(0);
            line.A.Y.Should().Be(0);

            line.B.X.Should().Be(10);
            line.B.Y.Should().Be(10);
        }

        [Test]
        public void Sets_Line_When_A_Is_Before_B_But_Below()
        {
            var a = new CgmPoint(0, 10);
            var b = new CgmPoint(10, 0);

            var line = new CgmLine(a, b);

            line.A.X.Should().Be(0);
            line.A.Y.Should().Be(10);

            line.B.X.Should().Be(10);
            line.B.Y.Should().Be(0);
        }

        [Test]
        public void Sets_Line_When_B_Is_Before_A()
        {
            var a = new CgmPoint(10, 10);
            var b = new CgmPoint(0, 0);

            var line = new CgmLine(a, b);

            line.A.X.Should().Be(0);
            line.A.Y.Should().Be(0);

            line.B.X.Should().Be(10);
            line.B.Y.Should().Be(10);
        }

        [Test]
        public void Sets_Line_When_A_Is_On_B()
        {
            var a = new CgmPoint(10, 0);
            var b = new CgmPoint(10, 10);

            var line = new CgmLine(a, b);

            line.A.X.Should().Be(10);
            line.A.Y.Should().Be(0);

            line.B.X.Should().Be(10);
            line.B.Y.Should().Be(10);
        }

        [Test]
        public void Sets_Line_When_A_Is_On_B_But_Below()
        {
            var a = new CgmPoint(10, 10);
            var b = new CgmPoint(10, 0);

            var line = new CgmLine(a, b);

            line.A.X.Should().Be(10);
            line.A.Y.Should().Be(0);

            line.B.X.Should().Be(10);
            line.B.Y.Should().Be(10);
        }
    }
}
