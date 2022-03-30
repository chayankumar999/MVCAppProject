
$(document).ready(function () {

    $(".switchBtn").on("click", (e) => {
        let clicked = (e.target.innerText == "Publish");
        if (clicked) {
            e.target.innerText = "Unpublish";
            $('#publishKey').val(1);
        } else {
            e.target.innerText = "Publish";
            $('#publishKey').val(0);
        }
        clicked = !clicked;
    });
});
