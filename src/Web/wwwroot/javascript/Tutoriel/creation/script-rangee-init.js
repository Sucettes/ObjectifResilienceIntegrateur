$(document).ready(function () {
    $('[data-addRangee]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        window.scriptRangeeUtils.ajoutRangee(event.currentTarget);
    });
    $('[data-supprimer]').on('click', window.scriptRangeeUtils.setDeleteClick);
    $('#btnConfirmDelete').click(() => window.scriptRangeeUtils.deleteRangee());
    $('[data-modifierTuto]').on('click', window.scriptRangeeUtils.setModificationClick);
    $('[data-modifierTutoConfirm]').click(() => window.scriptRangeeUtils.setModifierRangeeClick());
    $('[data-modifierTutoCancel]').on('click', window.scriptRangeeUtils.setModifierTutoCancelClick);

    window.addEventListener('beforeunload', () => delete window.scriptRangeeUtils);
});

// Obtient tous les boutons up des rangées
//let a = $('[data-up-tuto]')

// Affiche le form de tous les boutons récupérés
//a.each(x => console.log(a[x].form))

// Obtient le form au-dessus du bouton up cliqué de la rangée

// Obtient le formData de la rangée au-dessus du bouton cliqué

// Obtient le formData de la rangée du bouton cliqué

// Fait un new formData avec les deux forms

// Envoit le tout par une requête ajax
