import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Dropdown, Button } from "react-bootstrap";
import { PieChart, Settings, User, Briefcase } from "react-feather";
import PropTypes from "prop-types";
import userService from "../../services/userService";
import logger from "sabio-debug";
import Swal from "sweetalert2";
import { useNavigate } from "react-router";
import toastr from "toastr";
import "../../layouts/public/navbarpublic.css";
const _logger = logger.extend("PublicNavbarUser");

const PublicNavbarUser = (props) => {
  const navigate = useNavigate();
  const [userData, setUserData] = useState({
    currentUserData: {},
  });
  const signOut = () => {
    userService.signOut().then(onSignOutSuccess).catch(onSignOutError);
  };
  useEffect(() => {
    getCurrentUserById(props.currentUser.id);
  }, []);
  const getCurrentUserById = () => {
    userService
      .getCurrentById(props.currentUser.id)
      .then(onGetCurrentIdSuccess)
      .catch(onGetCurrentIdError);
  };

  const onGetCurrentIdSuccess = (data) => {
    _logger("currentIdSuccess", data);
    setUserData((prevState) => {
      const pData = { ...prevState };
      pData.currentUserData = data.item;

      return pData;
    });
  };
  const onGetCurrentIdError = (response) => {
    _logger("currentIdError", response);
  };

  const onSignOutSuccess = () => {
    Swal.fire({
      icon: "success",
      title: "You Have Signed Out",
    }).then(signedOut);
  };

  const signedOut = () => {
    navigate("/auth/signin", { state: { type: "LOGOUT" } });
  };

  const onSignOutError = (error) => {
    _logger("onSignOutError", error);
    toastr.error("There was a problem, Please try again");
  };
  _logger(props);
  return (
    <Dropdown className="nav-item" align="end">
      <span className="d-inline-block d-sm-none navpub-e">
        <Dropdown.Toggle as="a" className="nav-link">
          <Settings size={30} className="align-end gear" />
        </Dropdown.Toggle>
      </span>
      <span className="d-none d-sm-inline-block">
        <Dropdown.Toggle as="a" className="nav-link">
          <img
            src={userData.currentUserData.avatarUrl}
            className="avatarpub img-fluid rounded-circle mx-1"
            alt="avatarUrl"
          />
          <span className="text-dark ">
            {userData.currentUserData.firstName +
              " " +
              userData.currentUserData.lastName}
          </span>
        </Dropdown.Toggle>
      </span>
      <Dropdown.Menu drop="end" className="">
        <Dropdown.Item>
          <User size={18} className="align-middle me-2" />
          Profile
        </Dropdown.Item>
        <Dropdown.Item>
          <Link to="/workhistory" className="dropdown-link">
            <Briefcase size={18} className="align-middle me-2" />
            Work History
          </Link>
        </Dropdown.Item>
        <Dropdown.Item>
          <Link to="/dashboard/analytics" className="dropdown-link">
            <PieChart size={18} className="align-middle me-2" />
            Analytics
          </Link>
        </Dropdown.Item>
        <Dropdown.Divider />
        <Dropdown.Item>
          <Link to="/pages/settings" className="dropdown-link">
            Settings & Privacy
          </Link>
        </Dropdown.Item>
        <Dropdown.Item>Help</Dropdown.Item>
        <Dropdown.Item>
          <Button onClick={signOut} className="">
            Sign Out
          </Button>
        </Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );
};
PublicNavbarUser.propTypes = {
  currentUser: PropTypes.shape({
    isLoggedIn: PropTypes.bool,
    id: PropTypes.number,
  }),
};

export default PublicNavbarUser;
