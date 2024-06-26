﻿$(document).ready(function () {

    "use strict";

    $('.searchInput').on('input', function () {
        let search = $(this).val().trim();
        let url = "/admin/user/searchusers";

        if (search) {
            url += "?search=" + search; 
        }

        fetch(url)
            .then(response => response.text())
            .then(data => {
                $('#userList').html(data);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });

    $(document).on("click", "#inputadding", function (e) {
        e.preventDefault();

        if ($(".single-content-color .spec-base").length >= 7) {
            return;
        }

        let url = $(this).attr("href")
        fetch(url).then(response => {
            if (response.ok) {
                return response.text();
            }
        }).then(data => {
            $(".single-content-color").append(data);
        })
    })

    $(document).on("click", "#inputadding1", function (e) {
        e.preventDefault();

        if ($(".single-content .spec-base").length >= 4) {
            return;
        }

        let url = $(this).attr("href")
        fetch(url).then(response => {
            if (response.ok) {
                return response.text();
            }
        }).then(data => {
            $(".single-content").append(data);
        })
    })

    $(document).on('click', '#removeBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "If you remove it you can't get it back again!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.tbl-content').html(data)
                    });

                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )

            }
        })


    })

    $(document).on('click', '.restoreBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure you want to restore this category?',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {


                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.tbl-content').html(data)
                    });

                Swal.fire(
                    'Recovered!',
                    'Your file has been recovered.',
                    'success'
                )
            }
        })


    })

    $(document).on('click', '.restoreBtn1', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure you want to restore this category?',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {


                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.tbl-content').html(data)
                    });

                Swal.fire(
                    'Recovered!',
                    'Your file has been recovered.',
                    'success'
                )
            }
        })


    })


    $(document).on('click', '.removeBtn1', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "If you remove it you can't get it back again!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.tbl-content').html(data)
                    });

                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )

            }
        })


    })

    $(document).on('click', '.restoreProd', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure you want to restore this category?',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {


                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.tbl-content').html(data)
                    });

                Swal.fire(
                    'Recovered!',
                    'Your file has been recovered.',
                    'success'
                )
            }
        })
    })

    $(document).on('click', '.deleteProdImg', function (e) {
          e.preventDefault();

          Swal.fire({
              title: 'Are you sure?',
              text: "You won't be able to revert this!",
              icon: 'warning',
              showCancelButton: true,
              confirmButtonColor: '#3085d6',
              cancelButtonColor: '#d33',
              confirmButtonText: 'Yes, delete it!'
          }).then((result) => {
              if (result.isConfirmed) {

                  let url = $(this).attr('href');

                  fetch(url)
                      .then(res => res.text())
                      .then(data => {
                          $('.productImages').html(data)
                      });

                  Swal.fire(
                      'Deleted!',
                      'Your file has been deleted.',
                      'success'
                  )

              }
          })


      })


    $(document).on('click', '.removeLabel', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => {
                        $('.contentDynm').html(data)
                    });

                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )

            }
        })


    })

    // filter sort
    $(document).on("click", ".priceSortArea", function (e) {
        e.preventDefault()

        let url = $(this).attr("href")

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.tbl-content').html(data);
            })
    })

    $('.dropdown-menu li a').click(function () {
        $('.selected-option').text($(this).text());
    });


})
