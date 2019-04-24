type Person = { Name : string; Age : int }

module DB =
    let savePerson (p:Person) = Ok()
    let loadPerson cId = { Name = "Foo"; Age = 18 }

module Untestable =
    let validateThenSaveToDb p =
        if p.Age < 18 then Error "Too young!"
        else DB.savePerson p

    let orchestrator customerId =
        let p = DB.loadPerson customerId
        validateThenSaveToDb p

    let app() =
        orchestrator 123

module Hof =
    let validateThenSave savePerson p =
        if p.Age < 18 then Error "Too young!"
        else savePerson p

    let orchestrator load save customerId =
        let p = load customerId
        validateThenSave save p

    let app() =
        orchestrator DB.loadPerson DB.savePerson 123

module Bootstrap =
    let validateThenSave savePerson p =
        if p.Age < 18 then Error "Too young!"
        else savePerson p

    let orchestrator load save customerId =
        let p = load customerId
        save p

    let app() =
        let saveToDb = validateThenSave DB.savePerson
        orchestrator DB.loadPerson saveToDb 123

module Compose =
    let validate p =
        if p.Age < 18 then Error "Too young!"
        else Ok p

    let orchestrator load save customerId =
        let p = load customerId
        save p

    let app() =
        let validateThenSaveToDb = validate >> Result.bind DB.savePerson
        orchestrator DB.loadPerson validateThenSaveToDb 123
    
module CompositionToTheMax =
    let validate p =
        if p.Age < 18 then Error "Too young!"
        else Ok p
            
    let app() =
        let orchestrator = DB.loadPerson >> validate >> Result.bind DB.savePerson
        orchestrator 123