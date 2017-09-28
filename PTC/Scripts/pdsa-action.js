$(document).ready(function () {
    // Connect to ane elements tha have 'data-pdsa-action'
    $("[data-pdsa-action]").on("click", function (e) {
        e.preventDefault();
        // Fill in hidden fields with action to post back to model
        $("#EventAction").val($(this).data("pdsa-action"));
        // Submit form with hidden values filled in
        $("form").submit();
    });
})