import React, { useState, useEffect } from "react";
import debug from "sabio-debug";
import { getTypes } from "../../services/lookUpService";
import BlogForm from "./BlogForm";
import Swal from "sweetalert2";

function Blog() {
  const _logger = debug.extend("Blog");

  useEffect(() => {
    _logger("useEffect for blogs");
    getTypes(["blogTypes"]).then(onBlogsPageSuccess).catch(onBlogsPageError);
  }, []);

  const onBlogsPageSuccess = (data) => {
    _logger("blogPageSuccess", data);

    setBlogFormData((prevState) => {
      const pData = { ...prevState };
      pData.blogTypeId = data.item.blogTypes;
      _logger("blogPageSuccessData", pData);

      return pData;
    });
  };
  const onBlogsPageError = (error) => {
    Swal.fire({
      icon: "error",
      title: `Blog Page Error ${error}`,
      confirmButtonText: "Try Again",
    });
  };

  const [blogFormData, setBlogFormData] = useState({
    blogTypeId: [],
    title: "",
    subject: "",
    content: "",
    isPublished: false,
    imageUrl: "",
    datePublished: "",
  });

  return (
    <div className="container mt-6">
      <BlogForm blogFormData={blogFormData}></BlogForm>
    </div>
  );
}

export default Blog;
