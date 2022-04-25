(function () {
    'use strict';

    var currentDataItemList;
    var currentPosition = 1;
    var currTargetClick;
    var nbPage;
    let scriptRechercheUtils = {
        rechercherTuto: event => {
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
                        pagination.children[1].remove();
                    }
                }
                for (var i = 1; i <= nbPage; i++) {
                    pagination = document.getElementById("navPagination");
                    let pageLink = document.createElement('li');
                    pageLink.className = 'page-item';

                    let $pageLinkBtn = $('<button id="test" data-paginationBtn class="page-link" value="' + (i) + '">' + i + '</button>');
                    $pageLinkBtn.on('click', scriptRechercheUtils.setPaginationClick);
                    pageLink.appendChild($pageLinkBtn[0]);
                    pagination.insertBefore(pageLink, pagination.children[pagination.children.length -1])
                }

                //for (var i = 0; i < 9; i++) {
                for (var i = ((9 * currentPosition)-9); i < (currentPosition*9); i++) {
                    scriptRechercheUtils.creationCarteItem(i);
                }
            } else {
                let $msgAucunItem = $('<div class="alert alert-warning" role="alert" style="width:100%;"><p style="color:black">Aucun résultats trouvé!</p></div>');
                $('#zoneCardTuto').parent().append($msgAucunItem);
                //$('#zoneCardTuto').append($msgAucunItem);
            }
        },
        creationCarteItem: (i) => {
            let tutoData = currentDataItemList[i];
            // créé la carte
            let $a = $('<a class= "col"></a>');
            $a.attr('href', 'Tutoriel/Consultation?id=' + tutoData.id);

            let $div = $('<div class="shadow-lg rounded" style="background-color:#f1f9ee;"></div>');
            let $div2 = $('<div></div>');
            if (tutoData.lienImgBanniere !== null) {
                $div2.append('<img style="max-width:100%;height:13.125rem;" src="' + tutoData.lienImgBanniere + '" class="card-img-top" alt="...">');
            } else {
                $div2.append('<img style="max-width:100%;height:13.125rem;" src="/images/imgplaceholder.png" class="card-img-top" alt="...">');
            }
            let $divBody = $('<div class="card-body"></div>');

            $divBody.append($('<h4 class="card-title" style="text-align:center;word-break: break-all;">' + tutoData.titre + '</h4>'));
            if (tutoData.estPublier == true) {
                $divBody.append($('<p style="color:#38b000; font-weight: bold;">États: Publié</p>'));
            } else {
                $divBody.append($('<p style="color:#ff7733;font-weight: bold;">États: Non Publié</p>'));
            }

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
            scriptRechercheUtils.ajouterItem();
        }
    }

    window.scriptRechercheUtils = scriptRechercheUtils;
})();