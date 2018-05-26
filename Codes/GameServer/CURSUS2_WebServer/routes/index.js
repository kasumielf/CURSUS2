var express = require('express');
var router = express.Router();
var request = require('request');
var parseString = require('xml2js').parseString;
var dateFormat = require('dateformat');
var forecastData;

/* GET home page. */
var url = 'http://newsky2.kma.go.kr/service/SecndSrtpdFrcstInfoService2/ForecastGrib?_type=json';

var queryParams = '&' + encodeURIComponent('ServiceKey') + '=kCExWNC%2FUdnctfLYyfDDgH1Bw97mrKYLOpzbPAqoFNDZnjqxVwJFSPsGLLW1lNKxkndGsn0mItRwecJY%2F1oBtA%3D%3D';
queryParams += '&' + encodeURIComponent('nx') + '=' + encodeURIComponent('60');
queryParams += '&' + encodeURIComponent('ny') + '=' + encodeURIComponent('120');

var forecastUpdateInterval = setInterval(function () {
    var now = new Date();
    var before_date = new Date(now.getTime() - 1000 * 60 * 30);

    var timeParams = '&' + encodeURIComponent('base_date') + '=' + encodeURIComponent(dateFormat(before_date, "yyyymmdd"));
    timeParams += '&' + encodeURIComponent('base_time') + '=' + encodeURIComponent(dateFormat(before_date, "HHMM"));

    request({
        url: url + queryParams + timeParams,
        method: 'GET'
    }, function (error, response, body) {

        try
        {
            var json = JSON.parse(body);

            var data = json.response.body.items.item;

            if (data != null)
            {
                var forecast = [];

                var len = data.length;

                for (var i = 0; i < len; ++i)
                {
                    var value = {};

                    value.type = data[i].category;
                    value.value = data[i].obsrValue;

                    forecast.push(value);
                }

                forecastData = forecast;
            }
        }
        catch (ex)
        {
            console.log(ex);
        }
            
    });
}, 60000);

router.get('/forecast', function (req, res) {
    res.send(forecastData);
});

module.exports = router;