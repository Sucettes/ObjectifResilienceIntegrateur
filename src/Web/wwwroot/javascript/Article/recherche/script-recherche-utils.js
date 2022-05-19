(function () {
    'use strict';

    var currentDataItemList;
    var currentPosition = 1;
    var currTargetClick;
    var nbPage;
    let scriptRechercheUtils = {
        rechercherArticle: event => {
            $('#btn-pagination-before').off('click');
            $('#btn-pagination-before').on('click', scriptRechercheUtils.setPaginationClick);
            $('#btn-pagination-after').off('click')
            $('#btn-pagination-after').on('click', scriptRechercheUtils.setPaginationClick);
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
                        scriptRechercheUtils.obtenirDroit(scriptRechercheUtils.ajouterItem);
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
                        scriptRechercheUtils.obtenirDroit(scriptRechercheUtils.ajouterItem);
                    },
                    error: function () {
                        alert("un problème est survenu");
                    }
                });
            }
        },
        ajouterItem: (aDroitGestionContenue) => {
            var nbItem = currentDataItemList.length;
            var zone = document.getElementById('zoneCardTuto');
            if (document.getElementById('errMsgRech')) {
                document.getElementById('errMsgRech').remove();
            }
            while (zone.firstChild) {
                zone.removeChild(zone.firstChild);
            }

            if (nbItem > 0) {
                nbPage = Math.ceil(nbItem / 9);

                let pagination = document.getElementById("navPagination");
                if (pagination.children.length > 2) {
                    for (var j = 1; j < pagination.children.length; j++) {
                        pagination.children[1].remove();
                    }
                }
                for (var i = 1; i <= nbPage; i++) {
                    pagination = document.getElementById("navPagination");
                    let pageLink = document.createElement('li');
                    pageLink.className = 'page-item';

                    let $pageLinkBtn = $('<button id="test" data-paginationBtn class="page-link" style="background-color:#38b000;" value="' + (i) + '">' + i + '</button>');
                    $pageLinkBtn.on('click', scriptRechercheUtils.setPaginationClick);
                    pageLink.appendChild($pageLinkBtn[0]);
                    pagination.insertBefore(pageLink, pagination.children[pagination.children.length - 1])
                }

                let indicePage = 9 * (currentPosition - 1)
                for (var i = indicePage; i < indicePage + 9; i++) {
                    if (i === currentDataItemList.length) {
                        break
                    }
                    //**********************
                    scriptRechercheUtils.creationCarteItem(i, aDroitGestionContenue);
                }
            } else {
                if (document.getElementById('errMsgRech')) {
                    document.getElementById('errMsgRech').remove();
                }
                let $msgAucunItem = $('<div role="alert" style="width:100%;text-align: center;" id="errMsgRech"><p style="color:black">Aucun résultats trouvé!</p></div>');
                $('#zoneCardTuto').append($msgAucunItem);
            }

        },
        creationCarteItem: (i, aDroitGestionContenue) => {
            let articleData = currentDataItemList[i];
        //    // créé la carte
            let $a = $('<a class= "col"></a>');
            $a.attr('href', 'Article?id=' + articleData.id);
            // TODO : Ici retiré le cardPodcast si est pas a la bonne place et si ca marche pas....
            let $div = $('<div class="shadow-lg rounded cardPodcast" style="background-color:#f1f9ee;"></div>');
            let $div2 = $('<div></div>');
            if (articleData.lienImg !== null) {
                $div2.append('<img style="max-width:100%;height:13.125rem;" src="https://mediafileobjectifresiliance.s3.ca-central-1.amazonaws.com/' + articleData.lienImg + '" class="card-img-top" alt="...">');
            } else {
                $div2.append('<img style="max-width:100%;height:13.125rem;" src="/images/imgplaceholder.png" class="card-img-top" alt="...">');
            }
            let $divBody = $('<div class="card-body"></div>');

            $divBody.append($('<h4 class="card-title" style="text-align:center;word-break: break-all;">' + articleData.titre + '</h4>'));

        //    // Affiche publié ou non si est gestionaire de contenue.
            if (aDroitGestionContenue === true) {
                if (articleData.estPublier === true) {
                    $divBody.append($('<p style="color:#38b000; font-weight: bold;">État: Publié</p>'));
                } else {
                    $divBody.append($('<p style="color:#ff7733;font-weight: bold;">État: Non Publié</p>'));
                }
            }
        //    //$divBody.append($('<p>Catégorie : ' + tutoData.categorie.nom + '</p>'));

        //    //$divBody.append($('<p>' + tutoData.introduction.substring(0,200) + '</p>'));

            $div.append($div2);
            $div.append($divBody);

            $a.append($div);

            $('#zoneCardTuto').append($a);
        },
        setPaginationClick: event => {
            event.preventDefault();
            event.stopPropagation();
            currTargetClick = event.currentTarget;
            scriptRechercheUtils.changePage();
        },
        changePage: () => {
            if (currTargetClick.id == 'btn-pagination-before') {
                if (currentPosition > 1) {
                    currentPosition -= 1;
                }
            } else if (currTargetClick.id == 'btn-pagination-after') {
                if (currentPosition < nbPage) {
                    currentPosition += 1;
                }
            } else {
                currentPosition = Number(currTargetClick.value);
            }
            scriptRechercheUtils.obtenirDroit(scriptRechercheUtils.ajouterItem);
        },
        /**
         * Vérifie si l'utilisateur a les droits de gestion sur le contenue et exécute
         * la fonction fournis en callback.
         * @param {any} callback
         */
        obtenirDroit: (callback) => {
            $.ajax({
                type: 'POST',
                url: window.location.pathname + "?handler=ObtenirDroit",
                data: new FormData(document.getElementById('formFiltre')),
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    callback(data);
                },
                error: function () {
                    alert("un problème est survenu");
                }
            });
        }
    }

    window.scriptRechercheUtils = scriptRechercheUtils;
})();