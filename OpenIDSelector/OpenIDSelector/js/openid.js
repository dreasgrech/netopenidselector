(function($) {
    $(function() {
        var usernameIdentifier = '{u}',
            container = $(".openid"),
            providerList = container.find(".providers"),
            fieldUrl = container.find(".fullfield"),
            submitToServer = container.find(".submitopenid"),
            processLogin = container.find(".openid_submit"),
            loginContainer = container.find('fieldset'),
            title = loginContainer.find('label').eq(0),
            protocol = loginContainer.find('.protocol'),
            usernameInput = loginContainer.find('.openid_username'),
            preUsername = loginContainer.find('.preUsername'),
            postUsername = loginContainer.find('.postUsername'),
            openIDimage = loginContainer.find(".openid_img"),
            getTokens = function(url) {
                /*
                0: Protocol
                1: Pre Username
                2: Post Username
                */
                var usernameIndex = url.indexOf(usernameIdentifier), protocolEndPosition = url.indexOf('://') + 3;
                return [
                    url.substring(0, protocolEndPosition),
                    url.substring(protocolEndPosition, usernameIndex),
                    url.substring(usernameIndex + usernameIdentifier.length, url.length)
                ];
            },
            setImage = function(item) {
                var image = item.find('img').attr('src');
                openIDimage.attr('src', image);
            },            
            setFieldUrl = function(url) {
                fieldUrl.val(url);
            },
            elementTypes = {
                direct: function(item) {
                    loginContainer.fadeOut();
                    var url = item.find('span').text();
                    setFieldUrl(url);
                    submitToServer.click();
                },
                username: function(item) {
                    title.html('Enter your ' + item.attr('title') + ' Username');
                    setImage(item);
                    var url = item.find('span').text(),
                        params = getTokens(url);
                    protocol.html(params[0]);
                    preUsername.html(params[1]);
                    postUsername.html(params[2]);
                }
            },
            providers = providerList.find("li");

        providers.each(function() {
            var provider = $(this), provClass = provider.attr('class').toLowerCase();
            provider.click(function() {
                providers.each(function() {
                    $(this).removeClass('highlight');
                });
                provider.addClass('highlight');
                elementTypes[provClass](provider);
            });
        });

        processLogin.click(function() {
            var username = usernameInput.val();
            if (!username) {
                return;
            }
            usernameInput.attr('disabled', 'disabled');
            processLogin.attr('disabled', 'disabled');
            setFieldUrl(protocol.text() + preUsername.text() + username + postUsername.text());
            submitToServer.click();
        });

        providers.first().click(); //Activate the 1st Provider (OpenID)
    });
} (jQuery));