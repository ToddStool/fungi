// SPDX-License-Identifier: MIT
pragma solidity ^0.8.21;

import "./Erc20.sol";

abstract contract PoolCreatableErc20i is ERC20 {
    address internal _pool;
    uint256 internal _startTime;
    bool internal _feeLocked;
    address immutable _pairCreator;

    constructor(
        string memory name_,
        string memory symbol_,
        address pairCreator
    ) ERC20(name_, symbol_) {
        _pairCreator = pairCreator;
    }

    modifier lockFee() {
        _feeLocked = true;
        _;
        _feeLocked = false;
    }

    function launch(address poolAddress) external payable {
        require(msg.sender == _pairCreator);
        require(!isStarted());
        _pool = poolAddress;
        _startTime = block.timestamp;
    }

    function isStarted() internal view returns (bool) {
        return _pool != address(0);
    }

    function pool() external view returns (address) {
        return _pool;
    }
}
