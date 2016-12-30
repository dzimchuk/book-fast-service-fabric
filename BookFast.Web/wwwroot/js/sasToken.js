var BookFast;
(function (module) {
    var SASTokenProvider = (function () {
        function SASTokenProvider(tokenEndpoint, entityId) {
            var endpoint = tokenEndpoint;
            var id = entityId;

            this.getTokenEndpoint = function () {
                return endpoint;
            }

            this.getEntityId = function () {
                return id;
            }
        }

        SASTokenProvider.prototype.getSASToken = function (originalFileName) {
            var deferred = $.Deferred();

            $.getJSON('/api/' + this.getTokenEndpoint() + '/' + this.getEntityId() +
                '/image-token', { originalFileName: originalFileName })
                .done(function (result) {
                    deferred.resolve(result.url);
                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Error getting SAS token: " + err);
                    deferred.reject();
                });

            return deferred.promise();
        };

        return SASTokenProvider;
    })();

    module.SASTokenProvider = SASTokenProvider;

})(BookFast || (BookFast = {}));