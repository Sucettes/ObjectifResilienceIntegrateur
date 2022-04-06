'use strict';
$(document).ready(function () {
    // Création du tuto.
    var tokenVal;
    $('[data-addRangee]').on('click', event => {
        event.preventDefault();
        let curTarget = event.currentTarget;

        tokenVal = $('input:hidden[name="__RequestVerificationToken"]').val();

        var href = window.location.pathname + "?handler=AjoutRangee";
        $.ajax({
            type: 'POST',
            url: href,
            data: new FormData(curTarget.form),
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                ajoutRangeeReussie(data, curTarget);
            },
            error: function () {
                // 
            }
        });
    });

    function ajoutRangeeReussie(data, curTarget) {
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        //form.setAttribute("asp-page", "CreationTuto");
        form.className = "shadow-sm rounded";
        form.style = "background-color:#f1f9ee; padding:0.7rem; margin-bottom:1.2rem; margin-top:0.7rem;";
        //form.setAttribute('action' = '/Tutoriel/CreationTuto');

        var idRangeeInput = document.createElement("input");
        idRangeeInput.type = "text";
        idRangeeInput.name = "idRangeeVal";
        idRangeeInput.setAttribute("value", data.value.idRangee);
        idRangeeInput.setAttribute("hidden", "");

        var idtutoInput = document.createElement("input");
        idtutoInput.type = "text";
        idtutoInput.name = "idtutoVal";
        idtutoInput.setAttribute("value", data.value.idTutoP);
        idtutoInput.setAttribute("hidden", "");

        // ----- Contenue -----
        var divRow = document.createElement('div');
        divRow.className = "row";
        divRow.style = "display:flex; justify-content:space-between";

        var titre3 = document.createElement('h3');
        titre3.className = "col-6";
        titre3.style = "display:inline; text-align:left;";
        titre3.textContent = data.value.inputTitreEtape;

        var divBtnAction = document.createElement('div');
        divBtnAction.className = "col-6";
        divBtnAction.style = "display:inline; text-align:right;";

        // bouton action
        var btnUp = document.createElement('button');
        //btnUp.type = 'submit';
        btnUp.setAttribute('type', 'submit');
        btnUp.className = "btn btn-secondary";
        btnUp.style = "margin-right:0.2rem;";
        btnUp.innerHTML = '<img src="/icons/up-arrow.svg" />';
        btnUp.setAttribute('data-uptuto', '');

        var btnEdit = document.createElement('button');
        //btnEdit.type = 'submit';
        btnEdit.setAttribute('type', 'submit');
        btnEdit.className = "btn btn-primary"
        btnEdit.style = "margin-right:0.2rem;";
        btnEdit.innerHTML = '<img src="/icons/editIcon.svg" />';
        btnEdit.setAttribute('data-modifiertuto', '');

        var btnDelete = document.createElement('button');
        //btnDelete.type = 'submit';
        btnDelete.setAttribute('type', 'submit');
        btnDelete.className = "btn btn-danger";
        btnDelete.style = "margin-right:0.2rem;";
        btnDelete.innerHTML = '<img src="/icons/deleteIcon.svg" />';
        btnDelete.setAttribute('data-bs-toggle', 'modal');
        btnDelete.setAttribute('data-bs-target', '#modalValideDelete');
        btnDelete.setAttribute('data-supprimer', '');

        
        var btnDown = document.createElement('button');
        //btnDown = type = 'submit';
        btnDown.setAttribute('type', 'submit');
        btnDown.className = 'btn btn-secondary';
        btnDown.style = 'margin-right:0.2rem;';
        btnDown.innerHTML = '<img src="/icons/down-arrow.svg" />';
        btnDown.setAttribute('data-downtuto', '');

        var divBody = document.createElement('div');
        divBody.className = 'card-body row';

        if (data.value.imageUrl != null) {
            if (data.value.rangeeTexte != null) {
                if (data.value.positionImage === "left") {
                    var p = document.createElement('p');
                    p.className = 'card-text';
                    p.style = 'text-align:justify;';
                    p.textContent = data.value.rangeeTexte;

                    var img = document.createElement('img');
                    img.setAttribute('src', data.value.imageUrl);
                    img.className = 'card-img-top';
                    img.style = 'max-height:20rem; max-width:30rem;';
                    divBody.appendChild(img);

                    var div8 = document.createElement('div');
                    div8.className = 'col-md-8'
                    div8.appendChild(p);

                    var div4 = document.createElement('div');
                    div4.className = 'col-md-4';
                    div4.appendChild(img);

                    divBody.appendChild(div4);
                    divBody.appendChild(div8);
                }
                else {
                    var p = document.createElement('p');
                    p.className = 'card-text';
                    p.style = 'text-align:justify;';
                    p.textContent = data.value.rangeeTexte;

                    var img = document.createElement('img');
                    img.setAttribute('src', data.value.imageUrl);
                    img.className = 'card-img-top';
                    img.style = 'max-height:20rem; max-width:30rem;';
                    divBody.appendChild(img);

                    var div8 = document.createElement('div');
                    div8.className = 'col-md-8'
                    div8.appendChild(p);

                    var div4 = document.createElement('div');
                    div4.className = 'col-md-4';
                    div4.appendChild(img);

                    divBody.appendChild(div8);
                    divBody.appendChild(div4);
                }
            }
            else {
                var img = document.createElement('img');
                img.setAttribute('src', data.value.imageUrl);
                img.className = 'card-img-top';
                img.style = 'max-height:20rem; max-width:30rem;';
                divBody.appendChild(img);
            }
        }
        else {
            var p = document.createElement('p');
            p.className = 'card-text';
            p.style = 'text-align:justify;';
            p.textContent = data.value.rangeeTexte;
            divBody.appendChild(p);
        }

        // ---- ajouts dans la page ---
        divBtnAction.appendChild(btnUp);
        divBtnAction.appendChild(btnEdit);
        divBtnAction.appendChild(btnDelete);
        divBtnAction.appendChild(btnDown);

        divRow.appendChild(titre3);
        divRow.appendChild(divBtnAction);

        form.appendChild(idRangeeInput)
        form.appendChild(idtutoInput)
        form.appendChild(divRow);
        form.appendChild(divBody);

        var token = document.createElement('input');
        token.setAttribute('name', '__RequestVerificationToken');
        token.setAttribute('type', 'hidden');
        token.setAttribute('value', tokenVal);
        form.appendChild(token);

        document.getElementById('rangeeTutoSection').appendChild(form);
    }
});

