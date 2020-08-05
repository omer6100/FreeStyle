var LastFmKey = "e178f40cea3ee8e7deaaa8b69128089f";
var GoogleKey = "AIzaSyC6perw931as-fOkiSIsmVYyfeIwLyyur4";

function getAlbumInfo(title, artist) {
    $.getJSON(
        "https://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=" + LastFmKey + "&artist=" + artist + "&album=" + title + "&autocorrect=1&format=json",
        function(json) {
            var alt = json.album.name.split(" ").join("") + "AlbumArt";
            var src = json.album.image[4]["#text"];
            var util = {sum:0};

            $("#img").html("<img id='album-art' alt = " + alt + " src = " + src + ">");
            $("#bio").html(json.album.wiki.summary);

            json.album.tracks.track.forEach(function(song){
                this.sum += Number(song.duration);
                $("#tracklist").append("<tr><td>" + song.name + "</td><td>" + secondsToTime(song.duration) + "</td></tr>");
            }, util);
            

            $("#totalDuration").append("<tr class=table-secondary><td><b>Total Duration<b></td>" +
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

            $("#img").html("<img id='album-art' alt = " + alt + " src = " + src + ">");

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

function getVideo(keyword) {
    

    $.getJSON("https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1&type=video&q=" +
        keyword +
        "&key=" + GoogleKey,
        function(json) {
            var videoId = json.items[0].id.videoId;
            var embed = "https://www.youtube.com/embed/" + videoId;

            var iframe = "<iframe width='745' height='500' src='" +
                embed +
                "' frameborder='0' allow='accelerometer; autoplay; encrrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>";

            $(".youtube-video").html(iframe);
        });


}


function animateScore() {

    var rtg = $(".rating-avg").text();
    $({ Counter: 0 }).animate({
            Counter: $(".rating-avg").text()
        },
        {
            duration: 1000,
            easing: 'swing',
            step: function () {
                $(".rating-avg").text(this.Counter.toFixed(1));
            }
        });
    $(".rating-avg").html(rtg);
}

function embedTweet() {
    var twitter = "<a href='https://twitter.com/share?ref_src=twsrc%5Etfw' class='twitter-share-button' data-size='large' data-text='Check out this page on Freestyle - a Music Reviewing Site' data-show-count='false'>Tweet</a>" +
        "<script async src='https://platform.twitter.com/widgets.js' charset='utf-8'>" +
        "</scr" +
        "ipt>";

    $(".share").html(twitter);
}

