namespace Com.Guru.FunctionalSupport

open System
open System
open System
open System.Text.RegularExpressions

module StringF =
    let nullOrEmpty s = String.IsNullOrEmpty(s)
    
    let notNullNorEmpty s = nullOrEmpty s |> not
    
    let join (separator: string) (elements: string list) =
        String.Join(separator, elements |> List.toArray)
        
    let joinNoSeparator elements = join "" elements
    
    let format template ([<ParamArray>] elements: string list) =
        let casted = [| for el in elements |> Seq.cast<obj> -> el |]
        String.Format(format = template, args = casted)
    
    let split (separator: string) (s: string) = s.Split(separator) |> Array.toList
    
    let replace (oldValue: string) (newValue: string) (element: string) = element.Replace(oldValue, newValue)
    
    let regexReplace regex (sub: string) element =
        let r = new Regex(regex)
        r.Replace(element, sub)
    
    let substring i f (s: string) =        
        match s with
        | s when s.Length > i -> s.Substring(i, (s.Length - i) |> min f) |> Some
        | _ -> None
    
    let substringZero f = substring 0 f
    
    let substringDrop dropped s =
        match String.length s with
        | l when dropped >= l -> None
        | l -> s |> substring dropped (l - dropped)
    
    let substringLast size f =        
        match String.length f with
        | l when l < size -> None
        | l -> substring (l - size) size f
        
    let datePad pad x = x.ToString().PadLeft(pad, '0')
    
    let dayMonthPad x = datePad 2 x
    
    let yearPad x = datePad 4 x
    
    let private trimCharacters = [| ' '; '\n'; '\r' |]
    
    let trim (x: string) = x.Trim(trimCharacters)
    
    let trimEnd (x: string) = x.TrimEnd(trimCharacters)
    
    let trimStart (x: string) = x.TrimStart(trimCharacters)
        
    let asOption (s: string) =
        if nullOrEmpty s then None else Some s
        
    let toUpper (s: string) = s.ToUpper()
    
    let toLower (s: string) = s.ToLower()
        
    let capital s =
        substringZero 1 s
        |> Option.map toUpper
        |> Option.bind (fun f ->
            substringDrop 1 s
            |> Option.map (fun x -> [f; x]))
        |> Option.map joinNoSeparator
        
    