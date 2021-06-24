import PropTypes from "prop-types";

const discountProps = {
  location: PropTypes.shape({
    state: PropTypes.shape({
      payload: PropTypes.shape({ id: PropTypes.number.isRequired }),
      type: PropTypes.string,
    }),
  }),
  history: PropTypes.shape({
    push: PropTypes.func,
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string,
    }),
  }),
  currentUser: PropTypes.shape({
    roles: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number,
        name: PropTypes.string,
      })
    ),
  }),
};
export { discountProps };
