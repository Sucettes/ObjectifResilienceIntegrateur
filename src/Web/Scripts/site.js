"use strict";

var photos =
    [
        {
            "src": "imageArticle.jpg",
            "desc": "Comment fabriquer une maison pour chat"
        },
        {
            "src": "imageArticle1.jpg",
            "desc": "tutoriel de recyclage"
        },
        {
            "src": "imageArticle2.jpg",
            "desc": "fabriquer un panier avec un carton"
        }

    ];


var indice = 0;

/**
 * Affiche la photo en cours
 */
function rafraichirPhoto() {
    var image = document.getElementById("diapo");
    image.src = "images/" + photos[indice].src;

    document.getElementById("par-texte").innerText = photos[indice].desc;
}

/**
 * Avance à la prochaine photo
 */
function clicSuivant() {
    indice = (indice + 1) % photos.length;
    rafraichirPhoto();
}

/**
 * Recule à la photo précédente
 */
function clicPrecedent() {
    indice = (indice + photos.length - 1) % photos.length;
    rafraichirPhoto();
}


/**
 * Initialisation de la page
 */
function initialisation() {
    // Quelqu'un peut me dire pourquoi ?
    rafraichirPhoto();

    var bouton = document.getElementById("btn-suivant");
    bouton.addEventListener("click", clicSuivant, false);
    bouton.disabled = "";

    bouton = document.getElementById("btn-precedent");
    bouton.addEventListener("click", clicPrecedent, false);
    bouton.disabled = "";
}

window.addEventListener('load', initialisation, false);

