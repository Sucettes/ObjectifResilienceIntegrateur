////"use strict";

////let id = 0;
////let valideDelete = false;
////let divParentId = "";

////// CLick listener sur le bouton pour ajouté une rangee.
////$("#btnAddRangee").click(function () {
////    addRow();
////});

////function addRow() {
////    // Id de la rangee
////    id += 1;

////    // Création du HTML pour la rangee.
////    let divParent = document.createElement("div");
////    divParent.className = "row";
////    divParent.id = "rangeeData_" + id;
////    let divColParent = document.createElement("div");
////    divColParent.className = "col-12";

////    let divRowGauche = document.createElement("div");
////    divRowGauche.className = "row";
////    let divColGaucheRowGauche = document.createElement("div");
////    divColGaucheRowGauche.className = "col-4";
////    let img = document.createElement("img");
////    img.style.cssText = "width:100%;height:100%";
////    img.setAttribute("src", "/images/imgplaceholder.jpg");
////    let divColDroiteRowGauche = document.createElement("div");
////    divColDroiteRowGauche.className = "col-8"; // text box
////    let txtAera = document.createElement("textarea");
////    txtAera.setAttribute("rows", "10");
////    txtAera.className = "form-control";

////    let divRowDroite = document.createElement("div");
////    divRowDroite.className = "row";
////    let divColGaucheRowDroite = document.createElement("div");
////    divColGaucheRowDroite.className = "col-4"; // file inpute
////    let inputPicture = document.createElement("input");
////    inputPicture.type = "file";
////    inputPicture.className = "form-control";
////    let divColDroiteRowDroite = document.createElement("div");
////    divColDroiteRowDroite.className = "col-8"; // btn delete

////    let btnDelete = document.createElement("button");
////    btnDelete.className = "btn btn-danger";
////    btnDelete.textContent = "Supprimer ranger";
////    btnDelete.setAttribute("row", "rangeeData");
////    btnDelete.setAttribute("data-bs-toggle", "modal");
////    btnDelete.setAttribute("data-bs-target", "#modalValideDelete");

////    divRowDroite.appendChild(divColGaucheRowDroite);
////    divRowDroite.appendChild(divColDroiteRowDroite);
////    divColGaucheRowDroite.appendChild(inputPicture);
////    divColDroiteRowDroite.appendChild(btnDelete);

////    divRowGauche.appendChild(divColGaucheRowGauche);
////    divRowGauche.appendChild(divColDroiteRowGauche);
////    divColGaucheRowGauche.appendChild(img);
////    divColDroiteRowGauche.appendChild(txtAera);

////    divColParent.appendChild(divRowGauche);
////    divColParent.appendChild(divRowDroite);

////    divParent.appendChild(divColParent);

////    // Click listener sur le bouton pour supprimer la rangee.
////    btnDelete.addEventListener('click', function () {
////        valideDelete = true;
////        divParentId = divParent.id;
////        //EffacerRowData(divParent.id);
////    });

////    $('#sectionRowData').append(divParent);
////}

////$('#btnConfirmDelete').click(function () {
////    if (valideDelete == true && divParentId != "") {
////        EffacerRowData(divParentId);
////    }
////    divParentId = "";
////    valideDelete = false;
////});

////// Supprime une rangee de contenue avec sont ID
////function EffacerRowData(elementId) {
////    const rowData = document.getElementById(elementId);
////    rowData.remove();
////}