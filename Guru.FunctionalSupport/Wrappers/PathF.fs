namespace Com.Guru.FunctionalSupport
open System.IO

module PathF =
    let parent ss = Directory.GetParent(ss).FullName
    
    let combine radix dir = Path.Combine(radix, dir)
