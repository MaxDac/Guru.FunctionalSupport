namespace Com.Guru.FunctionalSupport.Tests
open NUnit.Framework

module AssertF =
    let areEqual a b = Assert.AreEqual(a, b)
    
    let areNotEqual a b = Assert.AreNotEqual(a, b)
    
    let isTrue (a: bool) = Assert.IsTrue(a)
    
    let isFalse (a: bool) = Assert.IsFalse(a)
    
    let fail message = Assert.Fail(message)
    
    
