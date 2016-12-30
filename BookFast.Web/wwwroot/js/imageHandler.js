var BookFast;
(function (module) {
    var ImageHandler = (function () {
        function ImageHandler(tokenEndpoint) {
            this.tokenEndpoint = tokenEndpoint;
            this.fileToUpload = null;
        }

        ImageHandler.prototype.initialize = function () {                   
            var addButton = $('.btn-add-image');
            addButton.attr('disabled', 'disabled');

            var fileSelector = $('#file');

            $('.btn-remove-image').click(function () {
                var imageUrl = $(this).data('image');

                $('[value="' + imageUrl + '"]').remove();
                $('[src="' + imageUrl + '"]').parent().remove();
            });

            fileSelector.change(this, function (e) {
                var files = e.target.files;

                e.data.fileToUpload = files[0];
                addButton.removeAttr('disabled');
            });

            addButton.click(this, function (e) {
                var id = $('#Id').val();
                var tokenProvider = new BookFast.SASTokenProvider(e.data.tokenEndpoint, id);
                var fileUploader = new BookFast.FileUploader(tokenProvider);
                fileUploader.upload(e.data.fileToUpload)
                .done(function (submitUri) {
                    addUIElements(submitUri);
                    alert('"' + e.data.fileToUpload.name + '" has been upload. Make sure to save the form.');
                })
                .fail(function () {
                    alert('Failed to upload image');
                })
                .always(function () {
                    addButton.attr('disabled', 'disabled');
                    fileSelector.val(null);
                });
            });

            function addUIElements(submitUri) {
                var uri = submitUri.substring(0, submitUri.indexOf('?'));
                $('#imageFields').append($('<input type="hidden" value="' + uri + '" name="Images" />'));
            }
        };

        return ImageHandler;
    })();

    module.ImageHandler = ImageHandler;

})(BookFast || (BookFast = {}));