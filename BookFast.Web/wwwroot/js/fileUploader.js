var BookFast;
(function (module) {
    var FileUploader = (function () {
        function FileUploader(sasTokenProvider) {
            var tokenProvider = sasTokenProvider;

            this.getSASTokenProvider = function () {
                return tokenProvider;
            }
        }

        FileUploader.prototype.upload = function (file) {
            var deferred = $.Deferred();

            this.getSASTokenProvider().getSASToken(file.name).done(function (url) {
                var state = createState(file, url);
                doUpload(state);
            })
            .fail(function () {
                deferred.reject();
            });

            function doUpload(state) {
                var reader = new FileReader();

                reader.onloadend = function (evt) {
                    if (evt.target.readyState == FileReader.DONE) {
                        var uri = state.submitUri + '&comp=block&blockid=' + state.blockIds[state.blockIds.length - 1];
                        var requestData = new Uint8Array(evt.target.result);
                        $.ajax({
                            url: uri,
                            type: "PUT",
                            data: requestData,
                            processData: false,
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('x-ms-blob-type', 'BlockBlob');
                                xhr.setRequestHeader('Content-Length', requestData.length);
                            }
                        })
                        .done(function () {
                            state.bytesUploaded += requestData.length;

                            var percentComplete = ((parseFloat(state.bytesUploaded) / parseFloat(state.file.size)) * 100).toFixed(2);
                            deferred.notify(percentComplete);

                            uploadFileInBlocks(reader, state);
                        })
                        .fail(function (jqxhr, textStatus, error) {
                            var err = textStatus + ", " + error;
                            console.log("Error uploading a chunk: " + err);

                            deferred.reject();
                        });
                    }
                };

                uploadFileInBlocks(reader, state);
            }

            function uploadFileInBlocks(reader, state) {
                if (state.totalBytesRemaining > 0) {
                    console.log("current file pointer = " + state.currentFilePointer + " bytes to read = " + state.blockSize);

                    var fileContent = state.file.slice(state.currentFilePointer, state.currentFilePointer + state.blockSize);

                    var blockId = state.blockIdPrefix + pad(state.blockIds.length, 6);
                    console.log("block id = " + blockId);
                    state.blockIds.push(btoa(blockId));

                    reader.readAsArrayBuffer(fileContent);

                    state.currentFilePointer += state.blockSize;
                    state.totalBytesRemaining -= state.blockSize;
                    if (state.totalBytesRemaining < state.blockSize) {
                        state.blockSize = state.totalBytesRemaining;
                    }
                } else {
                    commitBlockList(state);
                }
            }

            function commitBlockList(state) {
                var uri = state.submitUri + '&comp=blocklist';
                console.log(uri);
                var requestBody = '<?xml version="1.0" encoding="utf-8"?><BlockList>';
                for (var i = 0; i < state.blockIds.length; i++) {
                    requestBody += '<Latest>' + state.blockIds[i] + '</Latest>';
                }
                requestBody += '</BlockList>';
                console.log(requestBody);
                $.ajax({
                    url: uri,
                    type: "PUT",
                    data: requestBody,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('x-ms-blob-content-type', state.file.type);
                        xhr.setRequestHeader('Content-Length', requestBody.length);
                    }
                })
                .done(function () {
                    deferred.resolve(state.submitUri);
                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Error committing a block blob: " + err);

                    deferred.reject();
                });
            }

            function pad(number, length) {
                var str = '' + number;
                while (str.length < length) {
                    str = '0' + str;
                }
                return str;
            }

            function createState(file, submitUri) {
                var maxBlockSize = 256 * 1024;

                var fileSize = file.size;
                var blockSize = fileSize < maxBlockSize ? fileSize : maxBlockSize;
                var numberOfBlocks = fileSize % blockSize == 0 ? fileSize / blockSize : parseInt(fileSize / blockSize, 10) + 1;

                console.log("block size = " + blockSize);
                console.log("total blocks = " + numberOfBlocks);

                return {
                    file: file,
                    submitUri: submitUri,
                    blockIds: new Array(),
                    blockIdPrefix: "block-",
                    bytesUploaded: 0,
                    blockSize: blockSize,
                    numberOfBlocks: numberOfBlocks,
                    totalBytesRemaining: fileSize,
                    currentFilePointer: 0
                };
            }

            return deferred.promise();
        };

        return FileUploader;
    })();

    module.FileUploader = FileUploader;

})(BookFast || (BookFast = {}));