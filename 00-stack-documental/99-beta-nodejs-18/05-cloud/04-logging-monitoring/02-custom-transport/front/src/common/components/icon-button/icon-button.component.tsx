import React from 'react';
import Tooltip from '@material-ui/core/Tooltip';
import IconButton, { IconButtonProps } from '@material-ui/core/IconButton';

interface Props extends IconButtonProps {
  tooltip?: string;
}

export const IconButtonComponent: React.FunctionComponent<Props> = React.forwardRef(
  (props, ref) => {
    const { tooltip, children, ...otherProps } = props;

    return Boolean(tooltip) && !otherProps.disabled ? (
      <Tooltip title={tooltip}>
        <IconButton {...otherProps} ref={ref}>
          {children}
        </IconButton>
      </Tooltip>
    ) : (
      <IconButton {...otherProps} ref={ref}>
        {children}
      </IconButton>
    );
  }
);

IconButtonComponent.defaultProps = {
  color: 'primary',
};
