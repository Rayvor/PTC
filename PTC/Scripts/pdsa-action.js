$(document).ready(function () {
    // Connect to ane elements tha have 'data-pdsa-action'
    $("[data-pdsa-action]").on("click", function (e) {
        var deletelabel = '';
        var submit = true;
        e.preventDefault();
        // Check for Delete
        if ($(this).data("pdsa-action") === "delete") {
            deletelabel = $(this).data("pdsa-deletelabel");
            if (!deletelabel) {
                deletelabel = 'Record';
            }
            if (!confirm("Delete this " + deletelabel + "?")) {
                submit = false;
            }
        }
        // Fill in hidden fields with action to post back to model
        $("#EventAction").val($(this).data("pdsa-action"));
        $("#EventArgument").val($(this).data("pdsa-arg"));
        if (submit) {
            // Submit form with hidden values filled in
            $("form").submit();
        }
    });
})