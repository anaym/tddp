﻿using System;
using FluentAssertions;
using NUnit.Framework;

namespace Utility.Geometry.Tests
{
    [TestFixture]
    public class Size_Should
    {
        private Size size;

        [SetUp]
        public void SetUp()
        {
            var rnd = new Random();
            size = new Size(rnd.Next(0, 100), rnd.Next(0, 100));
        }

        [Test]
        public void CalculateArea_Correctly()
        {
            size.Area.Should().Be(size.Width * size.Height);
        }

        [Test]
        public void ThrowArgumentException_WhenNegativeDimension()
        {
            Assert.Throws<ArgumentException>(() => new Size(-1, 0));
        }

        [Test]
        public void HaveEqualHash_WithEqualSize()
        {
            var a = new Size(new Random().Next(10), new Random().Next(10));
            var b = new Size(a.Width, a.Height);
            a.GetHashCode().Should().Be(b.GetHashCode());
        }
        
        [Test]
        public void Equal_AnotherCreatedFromSameArguments()
        {
            var a = new Size(new Random().Next(10), new Random().Next(10));
            var b = new Size(a.Width, a.Height);
            a.Should().Be(b);
        }

        [Test]
        public void NotEqual_AnotherCreatedFromOtherArguments()
        {
            var a = new Size(new Random().Next(10), new Random().Next(10));
            var b = new Size(a.Width, a.Height + 100500);
            a.Should().NotBe(b);
        }
    }
}