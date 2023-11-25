// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function applyLinks() {
    const links = document.querySelectorAll("a.page-link");


    // Пройтись по каждому элементу и добавить обработчик события click
    links.forEach(link => {
        if (link.hasAttribute("href")) {
            return;
        }



        var href = link.getAttribute("ajax-href");

        link.addEventListener("click", function () {
            // Ваш код для обработки события click
            $.ajax({
                url: href,
                type: 'GET',
                success: function (result) {
                    $('#partialData').html(result);
                    applyLinks();
                }
            });

            console.log("Нажата ссылка");
        });
    });

}


applyLinks();