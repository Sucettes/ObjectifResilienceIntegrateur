(function () {
    'use strict';

    var currentDataItemList;
    var currentPosition = 1;
    var nbPage;
    let scriptRechercheUtils = {
        rechercherTuto: event => {
            if (event == undefined) {
                $.ajax({
                    type: 'POST',
                    url: window.location.pathname + "?handler=Recherche",
                    data: new FormData(document.getElementById('formFiltre')),
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        currentDataItemList = data.value;
                        scriptRechercheUtils.ajouterItem();
                    },
                    error: function () {
                        alert("un problème est survenu");
                    }
                });
            } else {
                $.ajax({
                    type: 'POST',
                    url: window.location.pathname + "?handler=Recherche",
                    data: new FormData(event.currentTarget.form),
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        currentDataItemList = data.value;
                        scriptRechercheUtils.ajouterItem();
                    },
                    error: function () {
                        alert("un problème est survenu");
                    }
                });
            }
        },
        ajouterItem: () => {
            var nbItem = currentDataItemList.length;

            var zone = document.getElementById('zoneCardTuto');
            while (zone.firstChild) {
                zone.removeChild(zone.firstChild);
            }

            if (nbItem > 0) {
                nbPage = Math.ceil(nbItem / 9);

                let pagination = document.getElementById("navPagination");
                if (pagination.children.length > 2) {
                    for (var j = 1; j < pagination.children.length; j++) {
                        console.log(pagination.children[j])
                        pagination.children[1].remove();
                    }
                }
                for (var i = 1; i <= nbPage; i++) {
                    pagination = document.getElementById("navPagination");
                    let pageLink = document.createElement('li');
                    pageLink.className = 'page-item';

                    let pageLinkBtn = document.createElement('button');
                    pageLinkBtn.className = "page-link";
                    pageLinkBtn.textContent = i;
                    pageLinkBtn.setAttribute('pageNumber', i-1)
                    pageLink.appendChild(pageLinkBtn);

                    pagination.insertBefore(pageLink, pagination.children[pagination.children.length -1])

                    //pagination.parentNode.insertBefore($pageLink, pagination.parentNode.children[pagination.count]);
                    // ajouts page
                    //$pageLink.on('click', scriptRechercheUtils.setPageChangeClick);
                }

                for (var i = 0; i < 9; i++) {
                    scriptRechercheUtils.creationCarteItem(i);
                }
                //for (var i in currentDataItemList) {
                //    scriptRechercheUtils.creationCarteItem(i);
                //}
            } else {
                let $msgAucunItem = $('<div class="alert alert-warning" role="alert" style="width:100%;"><p style="color:black">Aucun résultats trouvé!</p></div>');
                $('#zoneCardTuto').append($msgAucunItem);
            }
        },
        creationCarteItem: (i) => {
            let tutoData = currentDataItemList[i];
            console.log(tutoData)
            // créé la carte
            let $a = $('<a class= "col"></a>');
            $a.attr('href', 'Tutoriel/Consultation?id="' + tutoData.id);

            let $div = $('<div class="shadow-lg rounded" style="background-color:#f1f9ee;"></div>');
            let $div2 = $('<div></div>');
            if (tutoData.lienImgBanniere != "") {
                $div2.append('<img src="' + tutoData.lienImgBanniere + '" class="card-img-top" alt="...">');
            } else {
                $div2.append('<img src="~/images/imgplaceholder.png" class="card-img-top" alt="...">');
            }
            let $divBody = $('<div class="card-body"></div>');

            $divBody.append($('<h4 class="card-title" style="text-align:center;">' + tutoData.titre + '</h4>'));
            $div.append($div2);
            $div.append($divBody);

            $a.append($div);

            $('#zoneCardTuto').append($a);
        }
        //setPageChangeClick: event => {
        //    event.preventDefault();
        //    event.stopPropagation();

        //    console.log(event.currentTarget);
        //}
    }

    window.scriptRechercheUtils = scriptRechercheUtils;
})();