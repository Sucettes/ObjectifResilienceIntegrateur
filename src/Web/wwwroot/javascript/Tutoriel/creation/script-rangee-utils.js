
(function () {
    'use strict';

    /* global scriptRangeeUtils:true */
    let scriptRangeeUtils = {
        currTargetToDelete: null,
        currTargetToModifier: null,
        currTargetToMove: null,
        setRangeeUpClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToMove = event.currentTarget;
            scriptRangeeUtils.moveRangeeUp();
        },
        setRangeeDownClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToMove = event.currentTarget;
            scriptRangeeUtils.moveRangeeDown();
        },
        setDeleteClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToDelete = event.currentTarget;
        },
        setModifierTutoCancelClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToModifier = event.currentTarget;
            scriptRangeeUtils.clearFormModifierRangee();
        },
        setModifierRangeeClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToModifier = event.currentTarget;
            scriptRangeeUtils.modifierRangee();
        },
        setModificationClick: event => {
            event.preventDefault();
            event.stopPropagation();
            scriptRangeeUtils.currTargetToModifier = event.currentTarget;

            scriptRangeeUtils.ajoutFormModifierRangee();
        },
        ajoutRangeeReussie: (data, curTarget) => {
            let form = document.createElement("form");
            form.setAttribute("method", "post");
            form.setAttribute("asp-page", "CreationTuto");
            form.className = "shadow-sm rounded";
            form.style = "background-color:#f1f9ee; padding:0.7rem; margin-bottom:1.2rem; margin-top:0.7rem;";

            let idRangeeInput = document.createElement("input");
            idRangeeInput.type = "text";
            idRangeeInput.name = "idRangeeVal";
            idRangeeInput.setAttribute("value", data.value.idRangee);
            idRangeeInput.setAttribute("hidden", "");

            let idtutoInput = document.createElement("input");
            idtutoInput.type = "text";
            idtutoInput.name = "idtutoVal";
            idtutoInput.setAttribute("value", data.value.idTutoP);
            idtutoInput.setAttribute("hidden", "");

            // ----- Contenue -----
            let divRow = document.createElement('div');
            divRow.className = "row";
            divRow.style = "display:flex; justify-content:space-between";

            let titre3 = document.createElement('h3');
            titre3.className = "col-6";
            titre3.style = "display:inline; text-align:left;";
            titre3.textContent = data.value.inputTitreEtape;

            let divBtnAction = document.createElement('div');
            divBtnAction.className = "col-6";
            divBtnAction.style = "display:inline; text-align:right;";

            let $btnUp = $('<button data-up-tuto class="btn btn-secondary" style="margin-right:0.2rem;"><img src="/icons/up-arrow.svg" /></button>');
            $btnUp.on('click', scriptRangeeUtils.setRangeeUpClick);
            let $btnEdit = $('<button class="btn btn-primary" style="margin-right:0.2rem;" data-modifierTuto><img src="/icons/editIcon.svg" /></button>');
            $btnEdit.on('click', scriptRangeeUtils.setModificationClick);
            let $btnDelete = $('<button class="btn btn-danger" style="margin-right:0.2rem;" data-bs-toggle="modal" data-bs-target="#modalValideDelete" data-supprimer><img src="/icons/deleteIcon.svg" /></button>');
            $btnDelete.on('click', scriptRangeeUtils.setDeleteClick);
            let $btnDown = $('<button data-down-tuto class="btn btn-secondary" style="margin-right:0.2rem;"><img src="/icons/down-arrow.svg" /></button>');
            $btnDown.on('click', scriptRangeeUtils.setRangeeDownClick);

            let divBody = document.createElement('div');
            divBody.className = 'card-body row';

            //console.log(data.value)
            if (data.value.imageUrl != null) {
                if (data.value.rangeeTexte != null) {
                    if (data.value.positionImage === "left") {
                        let p = document.createElement('p');
                        p.className = 'card-text';
                        p.style = 'text-align:justify;';
                        p.textContent = data.value.rangeeTexte;

                        let img = document.createElement('img');
                        img.setAttribute('src', data.value.imageUrl);
                        img.className = 'card-img-top';
                        img.style = 'max-height:20rem; max-width:30rem;';
                        divBody.appendChild(img);

                        let div8 = document.createElement('div');
                        div8.className = 'col-md-8'
                        div8.appendChild(p);

                        let div4 = document.createElement('div');
                        div4.className = 'col-md-4';
                        div4.appendChild(img);

                        divBody.appendChild(div4);
                        divBody.appendChild(div8);
                    }
                    else {
                        let p = document.createElement('p');
                        p.className = 'card-text';
                        p.style = 'text-align:justify;';
                        p.textContent = data.value.rangeeTexte;

                        let img = document.createElement('img');
                        img.setAttribute('src', data.value.imageUrl);
                        img.className = 'card-img-top';
                        img.style = 'max-height:20rem; max-width:30rem;';
                        divBody.appendChild(img);

                        let div8 = document.createElement('div');
                        div8.className = 'col-md-8'
                        div8.appendChild(p);

                        let div4 = document.createElement('div');
                        div4.className = 'col-md-4';
                        div4.appendChild(img);

                        divBody.appendChild(div8);
                        divBody.appendChild(div4);
                    }
                }
                else {
                    console.log("image seulement")
                    let img = document.createElement('img');
                    img.setAttribute('src', data.value.imageUrl);
                    img.className = 'card-img-top';
                    img.style = 'max-height:20rem; max-width:30rem;';
                    divBody.appendChild(img);
                }
            }
            else {
                let p = document.createElement('p');
                p.className = 'card-text';
                p.style = 'text-align:justify;';
                p.textContent = data.value.rangeeTexte;
                divBody.appendChild(p);
            }

            // ---- ajouts dans la page ---
            divBtnAction.appendChild($btnUp[0]);
            divBtnAction.appendChild($btnEdit[0]);
            divBtnAction.appendChild($btnDelete[0]);
            divBtnAction.appendChild($btnDown[0]);

            divRow.appendChild(titre3);
            divRow.appendChild(divBtnAction);

            form.appendChild(idRangeeInput)
            form.appendChild(idtutoInput)
            form.appendChild(divRow);
            form.appendChild(divBody);

            let token = document.createElement('input');
            token.setAttribute('name', '__RequestVerificationToken');
            token.setAttribute('type', 'hidden');
            token.setAttribute('value', $('input:hidden[name="__RequestVerificationToken"]').val());
            form.appendChild(token);

            document.getElementById('rangeeTutoSection').appendChild(form);

            curTarget.form[1].value = '';
            curTarget.form[2].value = '';
            curTarget.form[3].value = '';
        },
        ajoutRangee: curTarget => {
            $.ajax({
                type: 'POST',
                url: window.location.pathname + "?handler=AjoutRangee",
                data: new FormData(curTarget.form),
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    scriptRangeeUtils.ajoutRangeeReussie(data, curTarget);
                },
                error: function () {
                    alert("un problème est survenu");
                }
            });
        },
        deleteRangee: () => {
            $.ajax({
                type: 'DELETE',
                url: window.location.pathname + "?handler=DeleteRange",
                data: new FormData(scriptRangeeUtils.currTargetToDelete.form),
                cache: false,
                contentType: false,
                processData: false,
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                success: function () {
                    scriptRangeeUtils.currTargetToDelete.form.parentElement.removeChild(scriptRangeeUtils.currTargetToDelete.form);
                },
                error: function () {
                    alert("un problème est survenu");
                }
            });
        },
        ajoutFormModifierRangee: () => {
            $.ajax({
                type: 'GET',
                url: window.location.pathname + "?handler=RangeeById",
                data: { 'idRangee': scriptRangeeUtils.currTargetToModifier.form[0].value },
                contentType: "application/json",
                dataType: 'json',
                success: function (data) {
                    scriptRangeeUtils.currTargetToModifier.form[2].className = 'btn btn-secondary disabled';
                    scriptRangeeUtils.currTargetToModifier.form[3].className = 'btn btn-primary disabled';
                    scriptRangeeUtils.currTargetToModifier.form[4].className = 'btn btn-danger disabled';
                    scriptRangeeUtils.currTargetToModifier.form[5].className = 'btn btn-secondary disabled';

                    let formRangeeModifier = document.createElement("form");
                    formRangeeModifier.setAttribute("method", "post");
                    formRangeeModifier.setAttribute("asp-page", "CreationTuto");

                    let inputIdRangee = document.createElement('input');
                    inputIdRangee.setAttribute('id', 'idRangee');
                    inputIdRangee.setAttribute('name', 'idRangee');
                    inputIdRangee.setAttribute('hidden', '');
                    inputIdRangee.setAttribute('value', scriptRangeeUtils.currTargetToModifier.form[0].value);

                    let inputIdTuto = document.createElement('input');
                    inputIdTuto.setAttribute('id', 'idTutoP');
                    inputIdTuto.setAttribute('name', 'idTutoP');
                    inputIdTuto.setAttribute('hidden', '');
                    inputIdTuto.setAttribute('value', scriptRangeeUtils.currTargetToModifier.form[1].value);

                    let spanTitre = document.createElement('span');
                    spanTitre.className = 'input-group-text';
                    spanTitre.setAttribute('id', 'inputTitreEtape');
                    spanTitre.textContent = 'Titre Étap';

                    let inputeTitre = document.createElement('input');
                    inputeTitre.setAttribute('name', 'inputTitreEtape');
                    inputeTitre.className = 'form-control';
                    inputeTitre.value = data.value.titre;

                    let divTitreRangee = document.createElement('div');
                    divTitreRangee.className = 'input-group input-group-sm mb-3';
                    divTitreRangee.appendChild(spanTitre)
                    divTitreRangee.appendChild(inputeTitre)

                    // contenue
                    let divRangee = document.createElement('div');
                    divRangee.className = 'row';

                    let divCol9 = document.createElement('div');
                    divCol9.className = 'col-md-9';

                    let textAera = document.createElement('textarea');
                    textAera.className = 'form-control';
                    textAera.setAttribute('aria-label', 'Texte de la rangee');
                    textAera.setAttribute('rows', '6');
                    textAera.setAttribute('name', 'rangeeTexte');
                    textAera.textContent = data.value.texte;

                    let inputImg = document.createElement('input');
                    inputImg.setAttribute('type', 'file');
                    inputImg.setAttribute('name', 'imageRangeeFile');
                    inputImg.setAttribute('id', 'inputImgBanierre');
                    inputImg.className = 'form-control form-control-sm';
                    inputImg.textContent = data.value.lienImg;
                    divCol9.appendChild(textAera);
                    divCol9.appendChild(inputImg);

                    let divCol3 = document.createElement('div');
                    divCol3.className = 'col-md-3';

                    let divRow2 = document.createElement('div');
                    divRow2.className = 'row';
                    divRow2.setAttribute('style', 'margin-left:4px;margin-right:4px;');

                    let pPositionImg = document.createElement('p');
                    pPositionImg.setAttribute('style', 'margin-bottom:0;');
                    pPositionImg.textContent = "Position de l'image";

                    let divRdGauche = document.createElement('div');
                    divRdGauche.className = 'form-check';

                    let inputRdGauche = document.createElement('input');
                    inputRdGauche.className = 'form-check-input';
                    inputRdGauche.setAttribute('type', 'radio');
                    inputRdGauche.setAttribute('value', 'left');
                    inputRdGauche.setAttribute('name', 'positionImage');
                    inputRdGauche.setAttribute('id', ('imageLeft' + scriptRangeeUtils.currTargetToModifier.form[0].value));

                    let labelGauche = document.createElement('label');
                    labelGauche.className = 'form-check-label';
                    labelGauche.setAttribute('for', ('imageLeft' + scriptRangeeUtils.currTargetToModifier.form[0].value));
                    labelGauche.setAttribute('style', 'margin:0;');
                    labelGauche.textContent = 'Gauche';

                    let divRdDroit = document.createElement('div');
                    divRdDroit.className = 'form-check';

                    let inputRdDroit = document.createElement('input');
                    inputRdDroit.className = 'form-check-input';
                    inputRdDroit.setAttribute('type', 'radio');
                    inputRdDroit.setAttribute('value', 'right');
                    inputRdDroit.setAttribute('name', 'positionImage');
                    inputRdDroit.setAttribute('id', ('imageRight' + scriptRangeeUtils.currTargetToModifier.form[0].value));

                    let labelDroite = document.createElement('label');
                    labelDroite.className = 'form-check-label';
                    labelDroite.setAttribute('for', ('imageRight' + scriptRangeeUtils.currTargetToModifier.form[0].value));
                    labelDroite.setAttribute('style', 'margin:0;');
                    labelDroite.textContent = 'Droite';

                    if (data.value.positionImg == "left") {
                        inputRdGauche.setAttribute('checked', '');
                    } else {
                        inputRdDroit.setAttribute('checked', '');
                    }

                    divRdGauche.appendChild(inputRdGauche);
                    divRdGauche.appendChild(labelGauche);
                    divRdDroit.appendChild(inputRdDroit);
                    divRdDroit.appendChild(labelDroite);


                    let divAction = document.createElement('div');
                    divAction.setAttribute('style', 'display:flex;justify-content:space-around;');

                    let $btnActionCancelModif = $('<button class="btn btn-danger" data-modifierTutoCancel><img src="/icons/close.svg" /></button>');
                    $btnActionCancelModif.on('click', scriptRangeeUtils.setModifierTutoCancelClick);

                    let $btnActionValideModif = $('<button class="btn btn-primary" data-modifierTutoConfirm><img src="/icons/check.svg" /></button>');
                    $btnActionValideModif.on('click', scriptRangeeUtils.setModifierRangeeClick);

                    divAction.appendChild($btnActionCancelModif[0]);
                    divAction.appendChild($btnActionValideModif[0]);

                    divRow2.appendChild(pPositionImg);
                    divRow2.appendChild(divRdGauche);
                    divRow2.appendChild(divRdDroit);
                    if (data.value.lienImg != "" & data.value.lienImg != null) {
                        // Modifié Image TODO : Faire disparaitre le checkbox quand ya pas dimage.....
                        let divModifierImage = document.createElement('div');
                        divModifierImage.className = 'form-check';
                        let inputCheckImageModif = document.createElement('input');
                        inputCheckImageModif.className = 'form-check-input';
                        inputCheckImageModif.setAttribute('type', 'checkbox');
                        inputCheckImageModif.setAttribute('value', 'true');
                        inputCheckImageModif.setAttribute('name', 'cbRetirerImage');
                        inputCheckImageModif.setAttribute('id', 'imgModifiable');
                        let labelCheckImgModif = document.createElement('label');
                        labelCheckImgModif.className = 'form-check-label';
                        labelCheckImgModif.setAttribute('for', 'imgModifiable');
                        labelCheckImgModif.setAttribute('style', 'margin:0;width:auto;');
                        labelCheckImgModif.textContent = "Retiré l'image";
                        divModifierImage.appendChild(inputCheckImageModif);
                        divModifierImage.appendChild(labelCheckImgModif);
                        divRow2.appendChild(divModifierImage);
                    }
                    divRow2.appendChild(divAction);

                    divCol3.appendChild(divRow2);

                    divRangee.appendChild(divCol9);
                    divRangee.appendChild(divCol3);
                    formRangeeModifier.appendChild(inputIdRangee);
                    formRangeeModifier.appendChild(inputIdTuto);
                    formRangeeModifier.appendChild(divTitreRangee);

                    formRangeeModifier.appendChild(divRangee);


                    let token = document.createElement('input');
                    token.setAttribute('name', '__RequestVerificationToken');
                    token.setAttribute('type', 'hidden');
                    token.setAttribute('value', $('input:hidden[name="__RequestVerificationToken"]').val());
                    formRangeeModifier.appendChild(token);

                    scriptRangeeUtils.currTargetToModifier.form.appendChild(formRangeeModifier)
                },
                error: function () {
                    alert("un problème est survenu");
                }
            });
        },
        modifierRangee: () => {
            $.ajax({
                type: 'PUT',
                url: window.location.pathname + "?handler=EditRangee",
                data: new FormData(scriptRangeeUtils.currTargetToModifier.form),
                cache: false,
                contentType: false,
                processData: false,
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                success: function (data) {
                    scriptRangeeUtils.currTargetToModifier.form.parentElement[2].className = 'btn btn-secondary';
                    scriptRangeeUtils.currTargetToModifier.form.parentElement[3].className = 'btn btn-primary';
                    scriptRangeeUtils.currTargetToModifier.form.parentElement[4].className = 'btn btn-danger';
                    scriptRangeeUtils.currTargetToModifier.form.parentElement[5].className = 'btn btn-secondary';

                    var tokenVal = $('input:hidden[name="__RequestVerificationToken"]').val();


                    let form = scriptRangeeUtils.currTargetToModifier.form.parentElement

                    while (form.firstChild) {
                        form.removeChild(form.firstChild);
                    }

                    let idRangeeInput = document.createElement("input");
                    idRangeeInput.type = "text";
                    idRangeeInput.name = "idRangeeVal";
                    idRangeeInput.setAttribute("value", data.value.idRangee);
                    idRangeeInput.setAttribute("hidden", "");

                    let idtutoInput = document.createElement("input");
                    idtutoInput.type = "text";
                    idtutoInput.name = "idtutoVal";
                    idtutoInput.setAttribute("value", data.value.idTutoP);
                    idtutoInput.setAttribute("hidden", "");

                    // ----- Contenue -----
                    let divRow = document.createElement('div');
                    divRow.className = "row";
                    divRow.style = "display:flex; justify-content:space-between";

                    let titre3 = document.createElement('h3');
                    titre3.className = "col-6";
                    titre3.style = "display:inline; text-align:left;";
                    titre3.textContent = data.value.inputTitreEtape;

                    let divBtnAction = document.createElement('div');
                    divBtnAction.className = "col-6";
                    divBtnAction.style = "display:inline; text-align:right;";

                    // bouton action
                    let btnUp = document.createElement('button');
                    btnUp.className = "btn btn-secondary";
                    btnUp.style = "margin-right:0.2rem;";
                    btnUp.innerHTML = '<img src="/icons/up-arrow.svg" />';
                    btnUp.setAttribute('data-up-tuto', '');

                    let $btnEdit = $('<button class="btn btn-primary" style="margin-right:0.2rem;" data-modifierTuto><img src="/icons/editIcon.svg" /></button>');
                    $btnEdit.on('click', scriptRangeeUtils.setModificationClick);

                    let $btnDelete = $('<button class="btn btn-danger" style="margin-right:0.2rem;" data-bs-toggle="modal" data-bs-target="#modalValideDelete" data-supprimer><img src="/icons/deleteIcon.svg" /></button>');
                    $btnDelete.on('click', scriptRangeeUtils.setDeleteClick);

                    let btnDown = document.createElement('button');
                    btnDown.className = 'btn btn-secondary';
                    btnDown.style = 'margin-right:0.2rem;';
                    btnDown.innerHTML = '<img src="/icons/down-arrow.svg" />';
                    btnDown.setAttribute('data-down-tuto', '');

                    let divBody = document.createElement('div');
                    divBody.className = 'card-body row';
                    if (data.value.imageUrl != null) {
                        if (data.value.rangeeTexte != null) {
                            if (data.value.positionImage === "left") {
                                let p = document.createElement('p');
                                p.className = 'card-text';
                                p.style = 'text-align:justify;';
                                p.textContent = data.value.rangeeTexte;

                                let img = document.createElement('img');
                                img.setAttribute('src', data.value.imageUrl);
                                img.className = 'card-img-top';
                                img.style = 'max-height:20rem; max-width:30rem;';
                                divBody.appendChild(img);

                                let div8 = document.createElement('div');
                                div8.className = 'col-md-8'
                                div8.appendChild(p);

                                let div4 = document.createElement('div');
                                div4.className = 'col-md-4';
                                div4.appendChild(img);

                                divBody.appendChild(div4);
                                divBody.appendChild(div8);
                            }
                            else {
                                let p = document.createElement('p');
                                p.className = 'card-text';
                                p.style = 'text-align:justify;';
                                p.textContent = data.value.rangeeTexte;

                                let img = document.createElement('img');
                                img.setAttribute('src', data.value.imageUrl);
                                img.className = 'card-img-top';
                                img.style = 'max-height:20rem; max-width:30rem;';
                                divBody.appendChild(img);

                                let div8 = document.createElement('div');
                                div8.className = 'col-md-8'
                                div8.appendChild(p);

                                let div4 = document.createElement('div');
                                div4.className = 'col-md-4';
                                div4.appendChild(img);

                                divBody.appendChild(div8);
                                divBody.appendChild(div4);
                            }
                        }
                        else {
                            let img = document.createElement('img');
                            img.setAttribute('src', data.value.imageUrl);
                            img.className = 'card-img-top';
                            img.style = 'max-height:20rem; max-width:30rem;';
                            divBody.appendChild(img);
                        }
                    }
                    else {
                        let p = document.createElement('p');
                        p.className = 'card-text';
                        p.style = 'text-align:justify;';
                        p.textContent = data.value.rangeeTexte;
                        divBody.appendChild(p);
                    }
                    // ---- ajouts dans la page ---
                    divBtnAction.appendChild(btnUp);
                    divBtnAction.appendChild($btnEdit[0]);
                    divBtnAction.appendChild($btnDelete[0]);
                    divBtnAction.appendChild(btnDown);

                    divRow.appendChild(titre3);
                    divRow.appendChild(divBtnAction);

                    form.appendChild(idRangeeInput)
                    form.appendChild(idtutoInput)
                    form.appendChild(divRow);
                    form.appendChild(divBody);

                    let token = document.createElement('input');
                    token.setAttribute('name', '__RequestVerificationToken');
                    token.setAttribute('type', 'hidden');
                    token.setAttribute('value', tokenVal);

                    form.appendChild(token);

                },
                error: function () {
                    alert("un problème est survenu");
                }
            });
        },
        clearFormModifierRangee: () => {
            scriptRangeeUtils.currTargetToModifier.form.parentElement[2].className = 'btn btn-secondary';
            scriptRangeeUtils.currTargetToModifier.form.parentElement[3].className = 'btn btn-primary';
            scriptRangeeUtils.currTargetToModifier.form.parentElement[4].className = 'btn btn-danger';
            scriptRangeeUtils.currTargetToModifier.form.parentElement[5].className = 'btn btn-secondary';
            scriptRangeeUtils.currTargetToModifier.form.parentElement.removeChild(scriptRangeeUtils.currTargetToModifier.form);
        },
        moveRangeeUp: () => {
            let curr = scriptRangeeUtils.currTargetToMove;
            let index = Array.from(curr.form.parentNode.children).indexOf(curr.form);

            let oldEtape = curr.form.parentNode.children[index];
            if (curr.form.parentNode.children.length > 1 && curr.form.parentNode.children[index - 1] != null) {
                if (oldEtape[1].value == curr.form.parentNode.children[index - 1][1].value) {
                    let oldIdRangee = oldEtape[0].value;
                    let newIdRangee = curr.form.parentNode.children[index - 1][0].value;
                    let token = oldEtape[6].value;

                    let formData = new FormData();
                    formData.append('IdOld', oldIdRangee);
                    formData.append('IdNew', newIdRangee);

                    $.ajax({
                        type: 'POST',
                        url: window.location.pathname + "?handler=SwitchRangeeTuto",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        headers: {
                            RequestVerificationToken: token
                        },
                        success: function () {
                            oldEtape[0].value = newIdRangee;
                            curr.form.parentNode.children[index - 1][0].value = oldIdRangee;
                            curr.form.parentNode.insertBefore(oldEtape, curr.form.parentNode.children[index - 1]);
                        },
                        error: function () {
                            alert("un problème est survenu");
                        }
                    });
                }
            }
        },
        moveRangeeDown: () => {
            let curr = scriptRangeeUtils.currTargetToMove;
            let index = Array.from(curr.form.parentNode.children).indexOf(curr.form);

            let oldEtape = curr.form.parentNode.children[index];
            if (curr.form.parentNode.children[index].nextElementSibling != null) {
                if (oldEtape[1].value == oldEtape.nextElementSibling[1].value) {
                    let oldIdRangee = oldEtape[0].value;
                    let newIdRangee = oldEtape.nextElementSibling[0].value;
                    let token = oldEtape[6].value;

                    let formData = new FormData();
                    formData.append('IdOld', oldIdRangee);
                    formData.append('IdNew', newIdRangee);

                    $.ajax({
                        type: 'POST',
                        url: window.location.pathname + "?handler=SwitchRangeeTuto",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        headers: {
                            RequestVerificationToken: token
                        },
                        success: function () {
                            oldEtape[0].value = newIdRangee;
                            oldEtape.nextElementSibling[0].value = oldIdRangee;
                            curr.form.parentNode.insertBefore(oldEtape, curr.form.parentNode.children[index + 1].nextSibling);                        },
                        error: function () {
                            alert("un problème est survenu");
                        }
                    });                    
                }
            }
        }
    }

    window.scriptRangeeUtils = scriptRangeeUtils;
})();