// This code runs when the DOM is ready 
$(document).ready(function () {
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () { sharePointReady(); });
});
function sharePointReady() {
    $('#message').text('Hello from my SharePoint app');
}

