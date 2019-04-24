type Person = { Name : string; Age : int }

module DB =
    let savePerson (p:Person) = Ok()
    let loadPerson cId = { Name = "Foo"; Age = 18 }

module Untestable =
    let validateThenSaveToDb p =
        if p.Age < 18 then Error "Too young!"
        else DB.savePerson p

    let orchestrator() =
        let p = DB.loadPerson 123
        validateThenSaveToDb p

    let app() =
        orchestrator()

module Hof =
    let validateThenSave savePerson p =
        if p.Age < 18 then Error "Too young!"
        else savePerson p

    let orchestrator load save =
        let p = load 123
        validateThenSave save p

    let app() =
        orchestrator DB.loadPerson DB.savePerson

module Bootstrap =
    let validateThenSave savePerson p =
        if p.Age < 18 then Error "Too young!"
        else savePerson p

    let orchestrator load save =
        let p = load 123
        save p

    let app() =
        let saveToDb = validateThenSave DB.savePerson
        orchestrator DB.loadPerson saveToDb

module Compose =
    let validate p =
        if p.Age < 18 then Error "Too young!"
        else Ok p

    let orchestrator load save =
        let p = load 123
        save p

    let app() =
        let validateThenSaveToDb = validate >> Result.bind DB.savePerson
        orchestrator DB.loadPerson validateThenSaveToDb
    
module CompositionToTheMax =
    let validate p =
        if p.Age < 18 then Error "Too young!"
        else Ok p
            
    let app() =
        let loadValidateSave = DB.loadPerson >> validate >> Result.bind DB.savePerson
        loadValidateSave 123