import React, { useState } from "react";
import { Formik, Field, Form, ErrorMessage, useFormik } from "formik";
import { Card, Form as RBForm, Button, Row, Col } from "react-bootstrap";
import propTypes from "prop-types";
import Swal from "sweetalert2";
import debug from "sabio-debug";
import BlogFormSchema from "../../schemas/blogFormSchema";
import { useNavigate } from "react-router-dom";
import BlogSide from "./BlogSide";
import blogService from "../../services/blogService";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

function BlogForm(props) {
  const _logger = debug.extend("Blog");

  const [showPreview, setShowPreview] = useState(false);
  const mapBlogTypeId = (blog) => (
    <option value={blog.id} key={`blog_${blog.id}`}>
      {blog.name}
    </option>
  );

  const previewClick = (values) => {//component, sidebar, dashboard items
    _logger(formikProps);
    setShowPreview((prevState) => !prevState);
    _logger(formikProps.values.isPublished);

    _logger("submit values", values);

    _logger("formikFormDataState");
  };

  const submitBlog = (values) => {
    _logger("data", values);
    if (values.content === "") {
      values.content = null;
    }
    if (values.imageUrl === "") {
      values.imageUrl = null;
    }
    if (values.isPublished === false) {
      values.datePublished = null;
    } else if (values.isPublished === true) {
      values.datePublished = new Date();
    }
    blogService.addBlog(values).then(onBlogAddSuccess).catch(onBlogAddError);
  };
  const navigate = useNavigate();
  const onBlogAddSuccess = (response) => {
    _logger("blogAddSuccess", response);
    Swal.fire({
      icon: "success",
      title: "Blog Inserted",
    });
    navigate(`/blogs`);
  };

  const onBlogAddError = (err) => {
    _logger("blogAddError", err);
    Swal.fire({
      icon: "error",
      title: "Incomplete Fields",
      confirmButtonText: "Try Again",
    });
  };
  const formikProps = useFormik({ initialValues: props.blogFormData });

  return (
    <Card>
      <Card.Header>
        <Card.Title>Blog Form</Card.Title>
      </Card.Header>
      <Card.Body>
        <Row>
          <Formik
            enableReinitialize={true}
            initialValues={props.blogFormData}
            onSubmit={previewClick}
            validationSchema={BlogFormSchema.blogFormSchema}
          >
            {({ values, setFieldValue }) => (
              <Form className="row">
                <Col>
                  <RBForm.Group className="mb-3">
                    <RBForm.Label>Blog Type</RBForm.Label>
                    <Field
                      as="select"
                      name="blogTypeId"
                      className="form-control"
                    >
                      <option value="0">Please select a blog type</option>
                      {props.blogFormData.blogTypeId.map(mapBlogTypeId)}
                    </Field>
                    <ErrorMessage
                      name="blogTypeId"
                      component="div"
                      className="danger"
                    ></ErrorMessage>
                  </RBForm.Group>
                  <RBForm.Group className="mb-3">
                    <RBForm.Label>Title</RBForm.Label>
                    <Field
                      type="text"
                      name="title"
                      className="form-control"
                    ></Field>
                    <ErrorMessage
                      name="title"
                      component="div"
                      className="danger"
                    ></ErrorMessage>
                  </RBForm.Group>
                  <RBForm.Group className="mb-3">
                    <RBForm.Label>Subject</RBForm.Label>
                    <Field
                      type="text"
                      name="subject"
                      className="form-control"
                    ></Field>
                    <ErrorMessage
                      name="subject"
                      component="div"
                      className="danger"
                    ></ErrorMessage>
                  </RBForm.Group>
                  <RBForm.Group className="mb-3">
                    <RBForm.Label>Content</RBForm.Label>
                    <CKEditor
                      editor={ClassicEditor}
                      onReady={(editor) => {
                        editor.editing.view.change((writer) => {
                          writer.setStyle(
                            "height",
                            "200px",
                            editor.editing.view.document.getRoot()
                          );
                        });
                      }}
                      onChange={(event, editor) => {
                        const data = editor.getData();
                        setFieldValue("content", data);
                      }}
                      name="content"
                      className="Form-control"
                    />
                    <ErrorMessage
                      name="content"
                      component="div"
                      className="danger"
                    ></ErrorMessage>
                  </RBForm.Group>
                  <RBForm.Group className="mb-3">
                    <RBForm.Label>Upload</RBForm.Label>
                    <Field
                      type="text"
                      name="imageUrl"
                      className="form-control"
                    ></Field>
                  </RBForm.Group>
                  <RBForm.Group className="mb-3">
                    <Field
                      type="checkbox"
                      name="isPublished"
                      className="form-check-input"
                    />
                    <RBForm.Label> Do you want to publish?</RBForm.Label>
                  </RBForm.Group>
                  {values.isPublished === true && (
                    <RBForm.Group className="mb-3 d-none">
                      <RBForm.Label>Date Published</RBForm.Label>
                      <Field
                        type="text"
                        name="datePublished"
                        className="form-control"
                      ></Field>
                    </RBForm.Group>
                  )}
                  <Button type="submit" className="btn btn-primary">
                    {showPreview ? "Hide Preview" : "Preview Blog"}
                  </Button>
                </Col>
                {showPreview === true && (
                  <Col>
                    <BlogSide
                      formikValues={values}
                      submitBlog={submitBlog}
                    ></BlogSide>
                  </Col>
                )}
              </Form>
            )}
          </Formik>
        </Row>
      </Card.Body>
    </Card>
  );
}

BlogForm.propTypes = {
  blogFormData: propTypes.shape({
    blogTypeId: propTypes.arrayOf(
      propTypes.shape({ id: propTypes.number, name: propTypes.string })
    ),
    title: propTypes.string.isRequired,
    subject: propTypes.string.isRequired,
    content: propTypes.string,
    isPublished: propTypes.bool.isRequired,
    imageUrl: propTypes.string,
    datePublished: propTypes.string || null,
  }),
};

export default BlogForm;
