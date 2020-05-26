var LastFmKey = "e178f40cea3ee8e7deaaa8b69128089f";

function getAlbumInfo(title, artist) {
    $.getJSON(
        "https://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=" + LastFmKey + "&artist=" + artist + "&album=" + title + "&autocorrect=1&format=json",
        function(json) {
            var alt = json.album.name.split(" ").join("") + "AlbumArt";
            var src = json.album.image[4]["#text"];
            var util = {sum:0};

            $("#img").html("<img alt = " + alt + " src = " + src + ">");
            $("#bio").html(json.album.wiki.summary);

            json.album.tracks.track.forEach(function(song){
                this.sum += Number(song.duration);
                $("#tracklist").append("<tr><td>" + song.name + "</td><td>" + secondsToTime(song.duration) + "</td></tr>");
            }, util);
            

            $("#totalDuration").append("<tr class=table-secondary><td>Total Duration</td>" +
                "<td>" +
                "<b>"+
                secondsToTime(util.sum) +
                "</b>"+
                "</td>" +
                "</tr>");
        });
}

function getArtistInfo(name) {
    $.getJSON("https://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=" + name + "&api_key="+ LastFmKey+"&autocorrect=1&limit=10&format=json",
        function (json) {
            var i = Math.floor(Math.random() * 10) % 10;
            var src = json.topalbums.album[i].image[3]["#text"];
            var alt = name + "Picture";

            $("#img").html("<img alt = " + alt + " src = " + src + ">");

        });

    $.getJSON("https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" +
            name +
            "&api_key=" +
            LastFmKey +
            "&autocorrect=1&format=json",
        function(json) {
            $("#bio").html(json.artist.bio.summary);
        });
}

function secondsToTime(seconds) {
    var minutes = Math.floor(seconds / 60);
    return "" + Math.floor(seconds / 60) + ":" + strPadLeft(seconds - minutes * 60, "0", 2);
}

function strPadLeft(string, padding, length) {
    return (new Array(length + 1).join(padding) + string).slice(-length);
}

//function addStars(rating) {
//    var starCount = Math.floor(rating);
//
//    for (var i = 0; i < 5; i++) {
//        if (i < starCount) {
//            $("#ratingStars").append("<span class='glyphicon glyphicon-star'></span>");
//        } else {
//            $("#ratingStars").append("<span class='glyphicon glyphicon-star-empty'></span>");
//        }
//    }
//}