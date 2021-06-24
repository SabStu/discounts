import {
  Card,
  Divider,
  Button,
  TextField,
  InputAdornment,
  Grid,
} from "@material-ui/core";
import { DataGrid } from "@material-ui/data-grid";
import { discountProps } from "./discountProps";
import { SearchOutlined } from "@material-ui/icons";
import React, { useState, useEffect } from "react";
// import debug from "sabio-debug";
import { Fragment } from "react";
import discountService from "../../services/discountService";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { formatDate } from "../../services/dateService";
import { onGlobalError } from "../../services/serviceHelpers";

export default function Discounts(props) {
  const [coupons, setCoupons] = useState([]);
  const [mounted, setMounted] = useState(true);
  const [selected, setSelected] = useState([]);
  const [rows, setRows] = useState([]);
  const [page, setPage] = useState(0);

  useEffect(() => {
    getCoupons();
    return function cleanup() {
      return mounted;
    };
  }, []);

  const getCoupons = () => {
    discountService
      .getAllCoupons()
      .then(onCouponsSuccess)
      .catch(onCouponsError);
  };
  const onCouponsError = (error) => {
    onGlobalError(error);
  };
  const onCouponsSuccess = (response) => {
    let coupons = response.item;
    let couponRows = coupons.map(mapCouponData);
    if (mounted) {
      setRows(couponRows);
      setCoupons(coupons);
      setMounted(false);
    }
  };

  const mapCouponData = (items) => {
    let newObj = {};
    newObj.id = items.id;
    newObj.title = items.title;
    newObj.description = items.description;
    newObj.couponCode = items.couponCode;
    newObj.isRedeemedAllowed = items.isRedeemedAllowed;
    newObj.percentage = items.percentage;
    newObj.listingId = items.listingId;
    newObj.validFrom = formatDate(items.validFrom);
    newObj.validUntil = formatDate(items.validUntil);
    return newObj;
  };

  const columns = [
    { field: "title", headerName: "Title", width: 180 },
    { field: "description", headerName: "Description", width: 260 },
    { field: "couponCode", headerName: "Code", width: 110 },
    { field: "isRedeemedAllowed", headerName: "Active", width: 93 },
    { field: "percentage", headerName: "Percentage", width: 130 },
    { field: "validFrom", headerName: "From", width: 160 },
    { field: "validUntil", headerName: "Until", width: 160 },
    { field: "listingId", hide: true },
    { field: "id", hide: true },
  ];

  function Display() {
    if (!coupons) {
      <div></div>;
    } else {
      return (
        <DataGrid
          autoHeight={true}
          rows={rows}
          disableSelectionOnClick={true}
          columns={columns}
          page={page}
          onPageChange={(params) => {
            setPage(params.page);
          }}
          pageSize={5}
          rowsPerPageOptions={[5, 10, 20]}
          pagination
          paginationMode="client"
          checkboxSelection
          onSelectionModelChange={(newSelection) => {
            setSelected(newSelection.selectionModel);
          }}
        />
      );
    }
  }

  const toastStyle = {
    position: "top-center",
    autoClose: 5000,
    hideProgressBar: true,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
  };

  const displayButtonError = () => {
    toast.error("Please Select One Item!", toastStyle);
  };

  const handleEditSelection = (id) => {
    if (id.length === 1) {
      const item = coupons.filter((x) => x.id === id[0]);
      const payload = item.map(mapCouponData);
      props.history.push(`/discountform/${String(id)}`, {
        payload: payload[0],
        type: "UPDATE",
      });
    } else {
      displayButtonError();
    }
  };

  const handleDeleteSelection = (id) => {
    if (id.length > 0) {
      let itemId = id[0];
      discountService
        .deleteCoupon(itemId)
        .then(onDeleteSuccess)
        .catch(onDeleteError);
    } else {
      displayButtonError();
    }
  };

  const onDeleteSuccess = (response) => {
    const filteredOut = coupons.filter((x) => {
      return x.id !== response;
    });
    const updatedCoupons = [...filteredOut];
    const mappedUpdated = updatedCoupons.map(mapCouponData);
    setRows(mappedUpdated);
    toast.success("Item Deleted!", toastStyle);
  };
  const onDeleteError = (err) => {
    onGlobalError(err);
    toast.error("Error!", toastStyle);
  };

  return (
    <Fragment>
      <Card className="p-4 mb-4">
        <div className="font-size-lg font-weight-bold">Coupons</div>
        <ToastContainer />
        <Divider className="my-4" />
        <Grid container justify="space-between">
          <Button
            variant="contained"
            color="primary"
            onClick={() => handleEditSelection(selected)}
          >
            Edit
          </Button>
          <Grid item md={6}>
            <TextField
              fullWidth={true}
              variant="outlined"
              size="small"
              placeholder="Search"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchOutlined />
                  </InputAdornment>
                ),
              }}
            />
          </Grid>
          <Button
            variant="contained"
            style={{ backgroundColor: "red", color: "white" }}
            onClick={() => handleDeleteSelection(selected)}
          >
            Delete
          </Button>
        </Grid>
        <Divider className="my-4" />
        {Display()}
        <Divider className="my-4" />
      </Card>
    </Fragment>
  );
}

Discounts.propTypes = discountProps;
