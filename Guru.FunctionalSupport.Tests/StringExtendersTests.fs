namespace Com.Guru.FunctionalSupport.Tests

open Com.Guru.FunctionalSupport
open NUnit.Framework

[<TestFixture>]
type StringExtendersTests() =
    [<Test>]
    member this.nullOrEmptyWhenNotEmptyTest() =
        let temp = "not-empty"
        
        StringF.nullOrEmpty temp |> AssertF.isFalse
        
    [<Test>]
    member this.nullOrEmptyWhenEmptyTest() =
        let temp: string = null
        
        StringF.nullOrEmpty temp |> AssertF.isTrue
        
    [<Test>]
    member this.joinTest() =
        let separator = ";"
        let elements = [ "this"; "is"; "the"; "array" ]
        let result = StringF.join separator elements
        
        AssertF.areEqual "this;is;the;array" result
        
    [<Test>]
    member this.joinEmptyArrayTest() =
        let separator = ";"
        let elements = []
        let result = StringF.join separator elements
        
        AssertF.areEqual "" result
        
    [<Test>]
    member this.formatTest() =
        let template = "This {0} is {1}"
        let elements = [ "string"; "formatted" ]
        
        try
            let result = StringF.format template elements
            AssertF.areEqual "This string is formatted" result
        with
        | e ->
            AssertF.fail (e.ToString())
        
    [<Test>]
    member this.splitTest() =
        let separator = ";"
        let temp = "this;is;the;string"
        let splitString = StringF.split separator temp
        
        match splitString with
        | [ first; second; third; fourth ] ->
            AssertF.areEqual "this" first
            AssertF.areEqual "is" second
            AssertF.areEqual "the" third
            AssertF.areEqual "string" fourth
        | _ -> AssertF.fail "String split wrongly"
        
    [<Test>]
    member this.splitWhenSeparatorNotFoundTest() =
        let separator = ","
        let temp = "this;is;the;string"
        let splitString = StringF.split separator temp
        
        match splitString with
        | [ only ] ->
            AssertF.areEqual "this;is;the;string" only
        | _ -> AssertF.fail "String split wrongly"
        
    [<Test>]
    member this.splitEmptyStringTest() =
        let separator = ","
        let temp = ""
        let splitString = StringF.split separator temp
        
        match splitString with
        | [ only ] ->
            AssertF.areEqual "" only
        | _ -> AssertF.fail "String split wrongly"
        
    [<Test>]
    member this.replaceTest() =
        let oldString = ";"
        let newString = ","
        let temp = "this;is;the;string;"
        let result = StringF.replace oldString newString temp
        
        AssertF.areEqual "this,is,the,string," result
        
    [<Test>]
    member this.replaceEmptyStringTest() =
        let oldString = ";"
        let newString = ","
        let temp = ""
        let result = StringF.replace oldString newString temp
        
        AssertF.areEqual "" result
        
    [<Test>]
    member this.replaceOldValueNotFoundTest() =
        let oldString = ","
        let newString = ";"
        let temp = "this;is;the;string;"
        let result = StringF.replace oldString newString temp
        
        AssertF.areEqual temp result
        
    [<Test>]
    member this.regexReplaceTest() =
        let regex = "(this is not)"
        let newValue = "this is"
        let temp = "this is not the right result"
        let result = StringF.regexReplace regex newValue temp
        
        AssertF.areEqual "this is the right result" result
        
    [<Test>]
    member this.regexReplaceEmptyStringTest() =
        let regex = "(this is not)"
        let newValue = "this is"
        let temp = ""
        let result = StringF.regexReplace regex newValue temp
        
        AssertF.areEqual temp result
        
    [<Test>]
    member this.regexReplaceRegexNotFoundTest() =
        let regex = "(this is not)"
        let newValue = "this is"
        let temp = "this is always the right result"
        let result = StringF.regexReplace regex newValue temp
        
        AssertF.areEqual temp result
            
    [<Test>]
    member this.substringTest() =
        let temp = "NSDES"
        let result = StringF.substring 2 3 temp
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual "DES" 
        
    [<Test>]
    member this.substringTestNoValue() =
        let temp = "NS"
        let result = StringF.substring 2 3 temp
        
        result |> Option.isNone |> AssertF.isTrue
        
    [<Test>]
    member this.substringTestExceed() =
        let temp = "NSDES"
        let result = StringF.substring 2 5 temp
        
        result |> Option.isSome |> AssertF.isTrue
        result |> Option.get |> AssertF.areEqual "DES"
        
    [<Test>]
    member this.SubstringLastTest() =
        let template = "This is to test the last"
        let result = StringF.substringLast 4 template
        
        AssertF.isTrue result.IsSome
        result |> Option.get |> AssertF.areEqual "last" 
        
    [<Test>]
    member this.SubstringLastLesserLengthTest() =
        let template = "ast"
        let result = StringF.substringLast 4 template
        
        AssertF.isTrue result.IsNone

    [<Test>]        
    member this.datePadTest() =
        let dateElement = "1"
        let result = StringF.datePad 2 dateElement
        
        AssertF.areEqual "01" result

    [<Test>]        
    member this.datePadWithMoreThanPadLengthTest() =
        let dateElement = "111"
        let result = StringF.datePad 2 dateElement
        
        AssertF.areEqual "111" result

    [<Test>]        
    member this.datePadWithEmptyStringTest() =
        let dateElement = ""
        let result = StringF.datePad 2 dateElement
        
        AssertF.areEqual "00" result
    
    [<Test>]
    member this.trimStringTest() =
        let temp = "this is the string \n"
        let result = StringF.trim temp
        
        AssertF.areEqual "this is the string" result
    
    [<Test>]
    member this.trimAlreadyTrimmedStringTest() =
        let temp = "this is the string"
        let result = StringF.trim temp
        
        AssertF.areEqual temp result
    
    [<Test>]
    member this.trimEmptyStringTest() =
        let temp = ""
        let result = StringF.trim temp
        
        AssertF.areEqual "" result
    
    [<Test>]
    member this.trimStartStringTest() =
        let temp = " \nthis is the string"
        let result = StringF.trimStart temp
        
        AssertF.areEqual "this is the string" result
    
    [<Test>]
    member this.trimStartWithStringToBeTrimmedLastTest() =
        let temp = "this is the string \n"
        let result = StringF.trimStart temp
        
        AssertF.areEqual "this is the string \n" result
    
    [<Test>]
    member this.trimStartAlreadyTrimmedStringTest() =
        let temp = "this is the string"
        let result = StringF.trimStart temp
        
        AssertF.areEqual temp result
    
    [<Test>]
    member this.trimStartEmptyStringTest() =
        let temp = ""
        let result = StringF.trimStart temp
        
        AssertF.areEqual "" result
    
    [<Test>]
    member this.trimEndStringTest() =
        let temp = "this is the string \n"
        let result = StringF.trimEnd temp
        
        AssertF.areEqual "this is the string" result
    
    [<Test>]
    member this.trimEndWithStringToBeTrimmedStartTest() =
        let temp = " \nthis is the string"
        let result = StringF.trimEnd temp
        
        AssertF.areEqual " \nthis is the string" result
    
    [<Test>]
    member this.trimEndAlreadyTrimmedStringTest() =
        let temp = "this is the string"
        let result = StringF.trim temp
        
        AssertF.areEqual temp result
    
    [<Test>]
    member this.trimEndEmptyStringTest() =
        let temp = ""
        let result = StringF.trim temp
        
        AssertF.areEqual "" result
        
