var key = "e178f40cea3ee8e7deaaa8b69128089f";

function getAlbum(title, artist) {
    $.getJSON(
        "https://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=" + key + "&artist=" + artist + "&album=" + title + "&autocorrect=1&format=json",
        function(json) {
            var alt = json.album.name.split(" ").join("");
            var src = json.album.image[4]["#text"];

            $("#img").html("<img alt = " + alt + " src = " + src + ">");

            $("#info").html("<h2><b>" + json.album.name + "</b></h2> by <b>" + json.album.artist + "</b>");
        });
}

function getArtist(name) {
    $.getJSON("https://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=" + name + "&api_key="+ key+"&autocorrect=1&limit=1&format=json",
        function (json) {

            var alt = json.topalbums.album[0].name.split(" ").join("");
            var src = json.topalbums.album[0].image[3]["#text"];

            $("#img").html("<img alt = " + alt + " src = " + src + ">");

            $("#info").html("<h2><b>" + json.topalbums.album[0].artist.name + "</b></h2>");

        });
}

