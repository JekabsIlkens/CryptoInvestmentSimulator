
// Function to show/hide username creation modal (pop-up).
function hideUsernameModal()
{
    $('#usernameCreationModal').modal('hide');
}

function showUsernameModal() {
    $('#usernameCreationModal').modal('show');
}

// Returns true if value contains only letters.
function isOnlyLetters(str)
{
    return /^[A-Za-z]*$/.test(str);
}

// Returns true if value is between 6 to 16 characters long.
function isValidLength(str)
{
    if (str.length >= 6 && str.length <= 16)
    {
        return true;
    }

    return false;
}

// Validates if entered username contains only letters and has length between 6 to 16.
// Doubles as protection against SQL injections.
$('#createForm').on('submit', function () {
    if (!isOnlyLetters($('#username').val()))
    {
        $('#usernameError').html("Username cannot contain numbers or special symbols!");
        return false;
    }
    else if (!isValidLength($('#username').val()))
    {
        $('#usernameError').html("Username must be between 6 to 16 characters long!");
        return false;
    }
    else
    {
        $('#usernameError').html("");
        hideUsernameModal();
        return true;
    }
});

$(document).ready(function ()
{
    showUsernameModal();
});