"use strict";

var btnAddRow = document.getElementById("btnAddRangee");

btnAddRow.addEventListener("click", function () {
    console.log("test-------");
    addElement();
})

function addElement() {
    var section = document.getElementById("sectionRowData");

    var divParent = document.createElement("div");
    divParent.className = "row";
    var divColParent = document.createElement("div");
    divColParent.className = "col-12";

    var divRowGauche = document.createElement("div");
    divRowGauche.className = "row";
    var divColGaucheRowGauche = document.createElement("div");
    divColGaucheRowGauche.className = "col-4";
    var img = document.createElement("img");
    img.style.cssText = "width:100%;height:100%";
    img.setAttribute("src", "/images/imgplaceholder.jpg");
    var divColDroiteRowGauche = document.createElement("div");
    divColDroiteRowGauche.className = "col-8"; // text box
    var txtAera = document.createElement("textarea");
    txtAera.setAttribute("rows", "10");
    txtAera.className = "form-control";

    var divRowDroite = document.createElement("div");
    divRowDroite.className = "row";
    var divColGaucheRowDroite = document.createElement("div");
    divColGaucheRowDroite.className = "col-4"; // file inpute
    var inputPicture = document.createElement("input");
    inputPicture.type = "file";
    inputPicture.className = "form-control";
    var divColDroiteRowDroite = document.createElement("div");
    divColDroiteRowDroite.className = "col-8"; // btn delete
    var btnDelete = document.createElement("button");
    btnDelete.className = "btn btn-danger";
    btnDelete.textContent = "Supprimer ranger";

    divRowDroite.appendChild(divColGaucheRowDroite);
    divRowDroite.appendChild(divColDroiteRowDroite);
    divColGaucheRowDroite.appendChild(inputPicture);
    divColDroiteRowDroite.appendChild(btnDelete);

    divRowGauche.appendChild(divColGaucheRowGauche);
    divRowGauche.appendChild(divColDroiteRowGauche);
    divColGaucheRowGauche.appendChild(img);
    divColDroiteRowGauche.appendChild(txtAera);

    divColParent.appendChild(divRowGauche);
    divColParent.appendChild(divRowDroite);

    divParent.appendChild(divColParent);

    // add the newly created element and its content into the DOM
    section.appendChild(divParent);
}

