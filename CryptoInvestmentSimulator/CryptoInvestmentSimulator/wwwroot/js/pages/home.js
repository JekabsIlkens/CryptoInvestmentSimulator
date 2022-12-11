
// Functions to show/hide new user popup.
function hideUsernameModal()
{
    $('#exampleModalCenter').modal('hide');
}

$(document).ready(function ()
{
    $('#exampleModalCenter').modal('show');

    document.getElementById("saveUsername").addEventListener("click", hideUsernameModal);
});