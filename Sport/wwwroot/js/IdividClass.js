$(document).ready(function () {
    $('#firstList').change(function () {
        var selectedValue = $(this).val();

        // Send AJAX request to the controller
        $.ajax({
            url: '/ClassInd/GetCoach',
            type: 'GET',
            data: { selectedValue: selectedValue },
            success: function (response) {
                // Update the second list with the retrieved data
                var secondList = $('#secondList');

                // Clear existing options
                secondList.empty();

                // Add new options from the response
                response.forEach(function (option) {
                    secondList.append('<option value="' + option.value + '">' + option.label + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.log('Error:', error);
            }
        });
    });
});
