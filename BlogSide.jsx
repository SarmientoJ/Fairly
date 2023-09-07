import React from "react";
import debug from "sabio-debug";
import { Button, Card } from "react-bootstrap";
import propTypes from "prop-types";
import DOMPurify from "dompurify";

function BlogSide(props) {
  const _logger = debug.extend("Blog");
  _logger("props", props);

  const publishCheck = () => {
    if (props.formikValues.isPublished === true) {
      props.formikValues.datePublished = new Date().toString();
    } else if (!props.formikValues.isPublished === false) {
      props.formikValues.datePublished = "";
    }
  };

  publishCheck();

  return (
    <Card>
      {props.formikValues.imageUrl && (
        <Card.Img
          width="50%"
          src={props.formikValues.imageUrl}
          alt="Card image cap"
        />
      )}
      <Card.Header>
        <Card.Title className="mb-0">{props.formikValues.title}</Card.Title>
        <Card.Title className="mb-0">{props.formikValues.subject}</Card.Title>
      </Card.Header>
      <Card.Body>
        <Card.Text
          dangerouslySetInnerHTML={{
            __html: DOMPurify.sanitize(props.formikValues.content),
          }}
        />
        <Card.Text>
          {" "}
          {props.formikValues.isPublished &&
            `Published On ${props.formikValues.datePublished}`}{" "}
        </Card.Text>
        <Button
          variant="primary"
          onClick={() => props.submitBlog(props.formikValues)}
        >
          Submit Blog
        </Button>
      </Card.Body>
    </Card>
  );
}

BlogSide.propTypes = {
  submitBlog: propTypes.func.isRequired,
  formikValues: propTypes.shape({
    blogTypeId: propTypes.string,
    title: propTypes.string,
    subject: propTypes.string,
    content: propTypes.string || null,
    isPublished: propTypes.bool,
    imageUrl: propTypes.string || null,
    datePublished: propTypes.string || null,
  }),
};
export default BlogSide;
