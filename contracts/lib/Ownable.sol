// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract Ownable {
    address _owner;

    event RenounceOwnership();

    constructor() {
        _owner = msg.sender;
    }

    modifier onlyOwner() {
        require(_owner == msg.sender, "only owner");
        _;
    }

    function owner() external view virtual returns (address) {
        return _owner;
    }

    function ownerRenounce() public onlyOwner {
        _owner = address(0);
        emit RenounceOwnership();
    }

    function transferOwnership(address newOwner) external onlyOwner {
        _owner = newOwner;
    }
}
