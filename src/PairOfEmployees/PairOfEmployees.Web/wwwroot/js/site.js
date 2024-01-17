$('form').submit(function (e) {
    e.preventDefault();

    var formData = new FormData(this);

    $.ajax({
        url: 'Home/UploadFile',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (result) {
            $('#resultContainer').html(result);
        }
    });

    this.reset();
});