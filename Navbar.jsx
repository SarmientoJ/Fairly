import React from "react";
import { Button, Navbar, Nav, Form, InputGroup } from "react-bootstrap";

import {
  AlertCircle,
  Bell,
  BellOff,
  Home,
  MessageCircle,
  UserPlus,
  Search,
  Moon,
  Sun,
} from "react-feather";

import useSidebar from "../../hooks/useSidebar";

import NavbarDropdown from "./NavbarDropdown";
import NavbarDropdownItem from "./NavbarDropdownItem";
import NavbarUser from "./NavbarUser";
import avatar7 from "../../assets/img/avatars/avatar-7.jpg";
import avatar8 from "../../assets/img/avatars/avatar-8.jpg";
import avatar9 from "../../assets/img/avatars/avatar-9.jpg";
import avatar10 from "../../assets/img/avatars/avatar-10.jpg";

import useTheme from "../../hooks/useTheme";
import { THEME } from "../../constants";

const notifications = [
  {
    type: "important",
    title: "Update completed",
    description: "Restart server 12 to complete the update.",
    time: "2h ago",
  },
  {
    type: "default",
    title: "Lorem ipsum",
    description: "Aliquam ex eros, imperdiet vulputate hendrerit et.",
    time: "6h ago",
  },
  {
    type: "login",
    title: "Login from 192.186.1.1",
    description: "",
    time: "6h ago",
  },
  {
    type: "request",
    title: "New connection",
    description: "Anna accepted your request.",
    time: "12h ago",
  },
];

const messages = [
  {
    name: "Ji-Young",
    avatar: avatar7,
    description: "Sunny Day. Sweepin' the clouds away.",
    time: "15m ago",
  },
  {
    name: "Maria and Elmo",
    avatar: avatar10,
    description: "On my way to where the air is sweet.",
    time: "2h ago",
  },
  {
    name: "Rosita",
    avatar: avatar8,
    description: "Can you tell my how to get",
    time: "4h ago",
  },
  {
    name: "Big Bird",
    avatar: avatar9,
    description: "How to get to Sesame Street.",
    time: "5h ago",
  },
];

const NavbarComponent = () => {
  const { isOpen, setIsOpen } = useSidebar();

  const { theme, setTheme } = useTheme();

  const changeTheme = () => {
    theme === THEME.DARK ? setTheme(THEME.DEFAULT) : setTheme(THEME.DARK);
  };

  const themeIcons = () => {
    return theme === THEME.DARK ? (
      <Sun size={18} className="feather" />
    ) : (
      <Moon size={18} className="feather" />
    );
  };

  return (
    <Navbar variant="light" expand className="navbar-bg">
      <span
        className="sidebar-toggle d-flex"
        onClick={() => {
          setIsOpen(!isOpen);
        }}
      >
        <i className="hamburger align-self-center" />
      </span>

      <Form inline="true" className="d-none d-sm-inline-block">
        <InputGroup className="input-group-navbar">
          <Form.Control placeholder={"Search"} aria-label="Search" />
          <Button variant="">
            <Search className="feather" />
          </Button>
        </InputGroup>
      </Form>

      <Navbar.Collapse>
        <Nav className="navbar-align">
          <Nav.Link onClick={changeTheme}>{themeIcons()}</Nav.Link>
          <NavbarDropdown
            header="New Messages"
            footer="Show all messages"
            icon={MessageCircle}
            count={messages.length}
            hasBadge
          >
            {messages.map((item, key) => {
              return (
                <NavbarDropdownItem
                  key={key}
                  icon={
                    <img
                      className="avatar img-fluid rounded-circle"
                      src={item.avatar}
                      alt={item.name}
                    />
                  }
                  title={item.name}
                  description={item.description}
                  time={item.time}
                  hasSpacing
                />
              );
            })}
          </NavbarDropdown>

          <NavbarDropdown
            header="New Notifications"
            footer="Show all notifications"
            icon={BellOff}
            count={notifications.length}
          >
            {notifications.map((item, key) => {
              let icon = <Bell size={18} className="text-warning" />;

              if (item.type === "important") {
                icon = <AlertCircle size={18} className="text-danger" />;
              }

              if (item.type === "login") {
                icon = <Home size={18} className="text-primary" />;
              }

              if (item.type === "request") {
                icon = <UserPlus size={18} className="text-success" />;
              }

              return (
                <NavbarDropdownItem
                  key={key}
                  icon={icon}
                  title={item.title}
                  description={item.description}
                  time={item.time}
                />
              );
            })}
          </NavbarDropdown>

          <NavbarUser />
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavbarComponent;
