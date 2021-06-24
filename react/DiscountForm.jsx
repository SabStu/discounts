import React from "react";
import { Fragment } from "react";
import { Formik, Form } from "formik";
import discountSchema from "../../schemas/discountSchema";
import discountService from "../../services/discountService";

import { getListingDetails } from "../../services/listingService";
// import debug from "sabio-debug";
import { discountProps } from "./discountProps";
import {
  Divider,
  Card,
  TextField,
  Button,
  Grid,
  FormControlLabel,
  Checkbox,
  FormControl,
  Select,
  MenuItem,
  InputLabel,
  FormHelperText,
} from "@material-ui/core";
import DateFnsUtils from "@date-io/date-fns";
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
} from "@material-ui/pickers";
import { onGlobalError, onGlobalSuccess } from "../../services/serviceHelpers";

// const _logger = debug.extend("DiscountForm");

const formData = {
  listingId: "",
  couponCode: "",
  title: "",
  description: "",
  percentage: "",
  validFrom: null,
  validUntil: null,
  isRedeemedAllowed: false,
};

class DiscountForm extends React.Component {
  state = {
    formData,
    couponCodes: [],
    listings: [],
    isUpdate: false,
    isSelected: false,
  };

  componentDidMount() {
    if (
      this.props.location.state &&
      this.props.location.state.type === "UPDATE"
    ) {
      this.updatedState();
    } else {
      this.listingDetails();
      this.coupons();
    }
  }

  updatedState = () => {
    let discounts = [this.props.location.state.payload];
    const newInValues = discounts.map(this.mapValues);
    this.setState({
      listings: newInValues,
      isUpdate: true,
    });
  };

  listingDetails = () => {
    getListingDetails().then(this.onListingSuccess).catch(this.onListingError);
  };
  onListingError = (error) => {
    onGlobalError(error);
  };
  onListingSuccess = (response) => {
    let mainListings = response.item;
    let activeListings = mainListings.filter(this.filterListingItems);
    let currentHostListings = activeListings.filter(this.filterHostItems);
    if (this.props.currentUser.roles[0].name === "Host") {
      this.setState({ listings: currentHostListings });
    } else {
      this.setState({ listings: activeListings });
    }
  };

  coupons = () => {
    discountService
      .getAllCoupons()
      .then(this.onCouponSuccess)
      .catch(this.onCouponError);
  };
  onCouponSuccess = (response) => {
    const arrOfCodes = response.item.map((x) => {
      return x.couponCode;
    });
    this.setState({ couponCodes: arrOfCodes });
  };

  onCouponError = (error) => {
    onGlobalError(error);
  };

  handleSubmit = (values, { resetForm }) => {
    values.listingId = parseInt(values.listingId);
    values.percentage = parseInt(values.percentage);
    values.validFrom = new Date(values.validFrom);
    values.validUntil = new Date(values.validUntil);
    if (
      this.props.location.state &&
      this.props.location.state.type === "UPDATE"
    ) {
      values.id = this.props.location.state.payload.id;
      discountService
        .updateCoupon(values, values.id)
        .then(this.onUpdateSuccess)
        .catch(this.onUpdateError);
    } else {
      discountService
        .createCoupon(values)
        .then(this.onCreateSucces)
        .catch(this.onCreateError);
    }
    resetForm(this.state.formData);
  };
  onCreateSucces = (response) => {
    onGlobalSuccess(response);
    this.props.history.push("/coupons");
  };
  onCreateError = (error) => {
    onGlobalError(error);
  };

  onUpdateSuccess = (response) => {
    onGlobalSuccess(response);
    this.props.history.push("/coupons");
  };
  onUpdateError = (error) => {
    onGlobalError(error);
  };

  handleDateChange = (date, setFieldValue, fieldName) => {
    setFieldValue(fieldName, new Date(date));
  };

  filterListingItems = (listing) => {
    if (listing.isActive === true) {
      return listing;
    }
  };
  filterHostItems = (listing) => {
    if (listing.createdBy.id === this.props.currentUser.id) {
      return listing;
    }
  };

  preSetDataOnSelected = (item, setFieldValue) => {
    if (this.state.isUpdate) {
      setFieldValue(item.target.name, item.target.value);
    } else {
      const target = item.target;
      const targetSelected = this.state.listings.filter(
        (x) => x.id === target.value
      );
      const validFrom = targetSelected[0].availabilityStart;
      const validUntil = targetSelected[0].availabilityEnd;
      setFieldValue(target.name, target.value);
      setFieldValue("validFrom", validFrom);
      setFieldValue("validUntil", validUntil);
    }
  };

  mapValues = (items) => {
    let newObj = {};
    newObj.title = items.title;
    newObj.description = items.description;
    newObj.couponCode = items.couponCode;
    newObj.isRedeemedAllowed = items.isRedeemedAllowed;
    newObj.percentage = items.percentage;
    newObj.listingId = items.listingId;
    newObj.validFrom = new Date(items.validFrom).toLocaleString();
    newObj.validUntil = new Date(items.validUntil).toLocaleString();
    return newObj;
  };

  mapMenuItems = (items) => {
    let propTest = false;
    if (items && items.listingId) {
      propTest = true;
    }
    return (
      <MenuItem
        key={items.id || items.listingId}
        value={propTest ? items.listingId : items.id}
      >
        {items.title}
      </MenuItem>
    );
  };

  render() {
    return (
      <Fragment>
        <Card className="p-4 mb-4">
          <div className="font-size-lg font-weight-bold">
            {this.state.isUpdate ? "Update Coupon" : "Create Coupon"}
          </div>
          <Divider className="my-4" />
          <div>
            <Formik
              enableReinitialize={true}
              initialValues={
                this.state.isUpdate
                  ? this.state.listings[0]
                  : this.state.formData
              }
              validateOnChange={true}
              validationSchema={discountSchema}
              onSubmit={this.handleSubmit}
            >
              {({ values, handleChange, touched, errors, setFieldValue }) => (
                <Form>
                  <Grid
                    container
                    direction="row"
                    justify="center"
                    alignItems="center"
                    spacing={4}
                  >
                    <Grid item sm={4}>
                      <FormControl fullWidth={true} variant="outlined">
                        <InputLabel
                          id="ListingName"
                          error={touched.listingId && Boolean(errors.listingId)}
                        >
                          Listings
                        </InputLabel>
                        <Select
                          label="Listings"
                          name="listingId"
                          labelId="ListingName"
                          value={values.listingId}
                          onChange={(selected) => {
                            this.preSetDataOnSelected(selected, setFieldValue);
                          }}
                          error={touched.listingId && Boolean(errors.listingId)}
                        >
                          {this.state.listings &&
                            this.state.listings.map(this.mapMenuItems)}
                        </Select>
                        <FormHelperText
                          error={touched.listingId && Boolean(errors.listingId)}
                        >
                          {touched.listingId && errors.listingId}
                        </FormHelperText>
                      </FormControl>
                    </Grid>
                    <Grid item>
                      <TextField
                        fullWidth={true}
                        name="couponCode"
                        label="Coupon Code"
                        value={values.couponCode}
                        variant="outlined"
                        onChange={handleChange}
                        error={touched.couponCode && Boolean(errors.couponCode)}
                        helperText={touched.couponCode && errors.couponCode}
                      />
                    </Grid>
                    <Grid item sm={4}>
                      <TextField
                        fullWidth={true}
                        name="title"
                        label="Title"
                        value={values.title}
                        variant="outlined"
                        onChange={handleChange}
                        error={touched.title && Boolean(errors.title)}
                        helperText={touched.title && errors.title}
                      />
                    </Grid>
                    <Grid item md={5}>
                      <TextField
                        multiline
                        name="description"
                        label="Descrption"
                        value={values.description}
                        onChange={handleChange}
                        variant="outlined"
                        rows={3}
                        fullWidth={true}
                        error={
                          touched.description && Boolean(errors.description)
                        }
                        helperText={touched.description && errors.description}
                      />
                    </Grid>
                    <Grid item md={2}>
                      <TextField
                        name="percentage"
                        label="Percentage"
                        value={values.percentage}
                        onChange={handleChange}
                        variant="outlined"
                        error={touched.percentage && Boolean(errors.percentage)}
                        helperText={touched.percentage && errors.percentage}
                      />
                    </Grid>
                    <Grid item md={3}>
                      <FormControlLabel
                        control={
                          <Checkbox
                            checked={values.isRedeemedAllowed}
                            onChange={handleChange}
                            name="isRedeemedAllowed"
                            color="primary"
                          />
                        }
                        label="Active"
                        labelPlacement="end"
                      />
                    </Grid>
                    <Grid item>
                      <MuiPickersUtilsProvider utils={DateFnsUtils}>
                        <KeyboardDatePicker
                          disableToolbar
                          variant="inline"
                          inputVariant="outlined"
                          margin="normal"
                          id="validFrom"
                          label="Date From"
                          name="validFrom"
                          format="MM-dd-yyyy"
                          value={values.validFrom}
                          onChange={(selected) =>
                            this.handleDateChange(
                              selected,
                              setFieldValue,
                              "validFrom"
                            )
                          }
                          KeyboardButtonProps={{
                            "aria-label": "change date",
                          }}
                        />
                      </MuiPickersUtilsProvider>
                    </Grid>
                    <Grid item sm={3}>
                      <MuiPickersUtilsProvider utils={DateFnsUtils}>
                        <KeyboardDatePicker
                          disableToolbar
                          variant="inline"
                          inputVariant="outlined"
                          margin="normal"
                          id="validUntil"
                          label="Date Until"
                          name="validUntil"
                          format="MM-dd-yyyy"
                          value={values.validUntil}
                          onChange={(selected) =>
                            this.handleDateChange(
                              selected,
                              setFieldValue,
                              "validUntil"
                            )
                          }
                          KeyboardButtonProps={{
                            "aria-label": "change date",
                          }}
                        />
                      </MuiPickersUtilsProvider>
                    </Grid>
                    <Grid item>
                      <Button variant="contained" color="primary" type="submit">
                        Submit
                      </Button>
                    </Grid>
                  </Grid>
                </Form>
              )}
            </Formik>
          </div>
        </Card>
      </Fragment>
    );
  }
}

DiscountForm.propTypes = discountProps;

export default DiscountForm;
