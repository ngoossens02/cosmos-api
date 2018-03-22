function genDocuments(quantity) {
    var context = getContext();
    var collection = context.getCollection();
    var results = [];
    for (i = 0; i < quantity; i++) {
        var accepted = collection.createDocument(collection.getSelfLink(),
            generateDocument(),
            function (err, documentCreated) {
                if (err) throw new Error('Error' + err.message);
                results.push(documentCreated)
            });
        if (!accepted) return;
    }
    context.getResponse().setBody(results);
    function generateDocument() {
        var firstNames = ["Cassie", "Mccall", "Estes", "Janice", "Cummings", "Viola", "Cherry", "Sandoval", "Roberta", "Elaine", "Richardson", "Velasquez", "Olivia", "Polly", "Jodi", "Burris", "Mays", "Susana", "Ana", "Leon", "Mcdonald", "Kirsten", "Diann", "Palmer", "Ernestine", "Bridgett", "Willis", "Doreen", "Workman", "Daniel", "Catalina", "Cheri", "Reid", "Lorna", "Roth", "Roxie", "Hodge", "Aurora", "Barker", "Shawna", "Hunter", "Doyle", "Mendoza", "Fitzpatrick", "David", "Georgina", "Neva", "Connie", "Wooten", "Mcclure"];
        var lastNames = ["Stevenson", "Riley", "Levy", "Hartman", "Patton", "Newman", "Holt", "Conner", "Petty", "Murray", "Caldwell", "Mcmillan", "Smith", "Key", "Ewing", "Potts", "Ware", "Kennedy", "Santana", "Gilliam", "Riggs", "Coleman", "Carney", "Espinoza", "Pate", "Eaton", "Rivers", "Hernandez", "Hobbs", "Terry", "Hunter", "Mathews", "Peck", "Craft", "Monroe", "Frank", "Jenkins", "Cannon", "Wall", "Simpson", "Rivas", "Myers", "Higgins", "Valdez", "Ellis", "Jacobs", "Leblanc", "Bean", "Mcconnell", "Welch"];
        var newDocument = {
            "firstName": firstNames[Math.floor(Math.random() * firstNames.length)],
            "lastName": lastNames[Math.floor(Math.random() * lastNames.length)]
        };
        if (Math.random() >= 0.5) {
            var companies = ["Netility", "Biotica", "Amtas", "Cofine", "Avit"];
            newDocument["company"] = companies[Math.floor(Math.random() * companies.length)];
        }
        if (Math.random() >= 0.5) {
            newDocument["isVested"] = (Math.random() >= 0.5);
        }
        if (Math.random() >= 0.5) {
            newDocument["age"] = Math.trunc(25 + (Math.random() * 40));
        }
        if (Math.random() >= 0.5) {
            newDocument["salary"] = Math.round((50000 + (Math.random() * 100000)) / 1000) * 1000;
        }
        return newDocument;
    }
}
