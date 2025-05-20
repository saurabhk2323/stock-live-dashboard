import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { clearNotification } from "../../redux/notificationSlice";
import "./NotificationBar.css";

const NotificationBar = () => {
  const dispatch = useDispatch();
  const { message, type } = useSelector((state) => state.notification);

  useEffect(() => {
    if (message) {
      const timer = setTimeout(() => {
        dispatch(clearNotification());
      }, 4000);
      return () => clearTimeout(timer);
    }
  }, [message, dispatch]);

  if (!message) return null;

  return (
    <div className={`notification-bar ${type}`}>
      <p>{message}</p>
    </div>
  );
};

export default NotificationBar;
