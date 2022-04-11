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
    $('[data-up-tuto]').on('click', window.scriptRangeeUtils.setRangeeUpClick);
    $('[data-down-tuto]').on('click', window.scriptRangeeUtils.setRangeeDownClick)

    window.addEventListener('beforeunload', () => delete window.scriptRangeeUtils);
});
